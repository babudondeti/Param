namespace Elexon.FA.BusinessValidation.DISBSADFlow.Command
{
    using Elexon.FA.BusinessValidation.DISBSADFlow.Validator;
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
    using Elexon.FA.BusinessValidation.Domain.Query;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.KeyVault.Abstractions;
    using Elexon.FA.Core.Logging;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ValidatedDisbsadCommandHandler" />
    /// </summary>
    public class ValidatedDisbsadCommandHandler : IRequestHandler<ValidatedDisbsadCommand, BusinessValidationProxy>
    {
        /// <summary>
        /// Defines the CoreKeyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient coreKeyVaultClient;

        /// <summary>
        /// Defines the _query
        /// </summary>
        private readonly IQueryFlow<Disbsad> query;

        /// <summary>
        /// Defines the _writer
        /// </summary>
        private readonly IProjectionWriter writer;

        /// <summary>
        /// Defines the _validator
        /// </summary>
        private readonly DisbsadValidator validator;

        /// <summary>
        /// Defines the outbound
        /// </summary>
        internal string outbound = string.Empty;

        /// <summary>
        /// Defines the rejected
        /// </summary>
        internal string rejected = string.Empty;

        /// <summary>
        /// Defines the businessValidationProxy
        /// </summary>
        internal BusinessValidationProxy businessValidationProxy = new BusinessValidationProxy();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedDisbsadCommandHandler"/> class.
        /// </summary>
        /// <param name="query">The query<see cref="IQueryFlow{Disbsad}"/></param>
        /// <param name="writer">The writer<see cref="IProjectionWriter"/></param>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public ValidatedDisbsadCommandHandler(IQueryFlow<Disbsad> query, IProjectionWriter writer, IApplicationBuilder applicationBuilder)
        {
            this.query = query;
            this.writer = writer;
            coreKeyVaultClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
            validator = new DisbsadValidator();
        }

        /// <summary>
        /// The Handle
        /// </summary>
        /// <param name="request">The request<see cref="ValidatedDisbsadCommand"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        public async Task<BusinessValidationProxy> Handle(ValidatedDisbsadCommand request, CancellationToken cancellationToken)
        {
            if (request?.Item != null)
            {
                var disbsads = await query.GetListAsync(request.Item.ItemPath, request.Item.ItemId);

                if (disbsads != null && disbsads.Any())
                {
                    var aggregate = new Aggregate<Disbsad>(request.Item, disbsads, null, null)
                    {
                        LongDay = DateTime.ParseExact(BusinessValidationConstants.CONFIG_LONGDAY, BusinessValidationConstants.CONFIG_DATEFORMAT, System.Globalization.CultureInfo.InvariantCulture),
                        ShortDay = DateTime.ParseExact(BusinessValidationConstants.CONFIG_SHORTDAY, BusinessValidationConstants.CONFIG_DATEFORMAT, System.Globalization.CultureInfo.InvariantCulture),
                    };

                    businessValidationProxy = await ValidateDisbsadAsync(aggregate);
                }
            }

            return businessValidationProxy;
        }

        /// <summary>
        /// The ValidateNetbsadAsync
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        private async Task<BusinessValidationProxy> ValidateDisbsadAsync(Aggregate<Disbsad> aggregate)
        {
            outbound = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_PROCESSING);
            rejected = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_REJECTED);

            try
            {
                validator.ValidateAndThrow(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);
                var warningResult = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                if (aggregate.ValidFlow.Any())
                {
                    await UploadDisBsadRecordsAsSucceeded(aggregate);
                }

                if (aggregate.InValidFlow.Any())
                {
                    warningResult.LogErrorMessage();
                    await UploadAndPublishRecordsAsRejected(aggregate);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                aggregate.ErrorMessage = ex.Message;
                businessValidationProxy.ErrorMessages.Add(ex.Message);
            }

            return businessValidationProxy;
        }

        /// <summary>
        /// The UploadAndPublishRecordsAsRejected
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="Task"/></returns>       
        private async Task UploadAndPublishRecordsAsRejected(Aggregate<Disbsad> aggregate)
        {
            var uniqueSettlementDateAndPeriods = aggregate.InValidFlow.GroupBy(g => new { g.SettDate, g.SettlementPeriod });

            foreach (var item in uniqueSettlementDateAndPeriods)
            {
                string path = aggregate.Item.ItemPath;
                var outputPath = Common.GetOutputFilePath(item.Select(x => x.SettDate).FirstOrDefault(),
                                    item.Select(x => x.SettlementPeriod).FirstOrDefault(), path, rejected);
                await UploadFileAndFileStatusForRejected(aggregate, outputPath);
                businessValidationProxy.InValidPaths.Add(outputPath);
            }
            Log.Information(BusinessValidationConstants.DISBSAD_INFO_MSG_INVALIDRECORDUPLOADEDTOREJECTEDFOLDER);
        }

        private async Task UploadFileAndFileStatusForRejected(Aggregate<Disbsad> aggregate, string outputPath)
        {
            await writer.UpLoadFile(aggregate.InValidFlow, outputPath);
            await writer.UpdateFileStatus(Common.GetStatusOfRejectedFile(outputPath, BusinessValidationConstants.REJECTED));
            businessValidationProxy.InValid = true;
        }

        /// <summary>
        /// The UploadDisBsadRecordsAsSucceeded
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task UploadDisBsadRecordsAsSucceeded(Aggregate<Disbsad> aggregate)
        {
            var uniqueSettlementDateAndPeriods = aggregate.ValidFlow.GroupBy(g => new { g.SettDate, g.SettlementPeriod });

            foreach (var item in uniqueSettlementDateAndPeriods)
            {
                string path = aggregate.Item.ItemPath;
                var outputPath = Common.GetOutputFilePath(item.Select(x => x.SettDate).FirstOrDefault(),
                                    item.Select(x => x.SettlementPeriod).FirstOrDefault(), path, outbound);
                await UploadFileAndFileStatus(aggregate, outputPath);
                businessValidationProxy.ValidPaths.Add(outputPath);
            }
            Log.Information(BusinessValidationConstants.DISBSAD_INFO_MSG_FILEUPLOADEDTOPROCESSINGFOLDER);
        }

        /// <summary>
        /// The UploadFileAndFileStatus
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <param name="outputPath">The outputPath<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task UploadFileAndFileStatus(Aggregate<Disbsad> aggregate, string outputPath)
        {
            await writer.UpLoadFile(aggregate.ValidFlow, outputPath);
            await writer.UpdateFileStatus(Common.GetStatusOfBusinessValidation(outputPath));
            businessValidationProxy.Valid = true;
        }
    }
}
