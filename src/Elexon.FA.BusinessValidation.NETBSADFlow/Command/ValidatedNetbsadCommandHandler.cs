namespace Elexon.FA.BusinessValidation.NETBSADFlow.Command
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
    using Elexon.FA.BusinessValidation.Domain.Query;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.BusinessValidation.NETBSADFlow.Validator;
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
    /// Defines the <see cref="ValidatedNetbsadCommandHandler" />
    /// </summary>
    public class ValidatedNetbsadCommandHandler : IRequestHandler<ValidatedNetbsadCommand, BusinessValidationProxy>
    {
        /// <summary>
        /// Defines the _coreKeyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient coreKeyVaultClient;

        /// <summary>
        /// Defines the _query
        /// </summary>
        private readonly IQueryFlow<Netbsad> query;

        /// <summary>
        /// Defines the _writer
        /// </summary>
        private readonly IProjectionWriter writer;

        /// <summary>
        /// Defines the _validator
        /// </summary>
        private readonly NetbsadValidator validator = new NetbsadValidator();

        /// <summary>
        /// Defines the inbound
        /// </summary>
        internal string inbound = string.Empty;

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
        /// Initializes a new instance of the <see cref="ValidatedNetbsadCommandHandler"/> class.
        /// </summary>
        /// <param name="query">The query<see cref="IQueryFlow{Netbsad}"/></param>
        /// <param name="writer">The writer<see cref="IProjectionWriter"/></param>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public ValidatedNetbsadCommandHandler(IQueryFlow<Netbsad> query, IProjectionWriter writer, IApplicationBuilder applicationBuilder)
        {
            this.query = query;
            this.writer = writer;
            coreKeyVaultClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
        }

        /// <summary>
        /// The Handle
        /// </summary>
        /// <param name="request">The request<see cref="ValidatedNetbsadCommand"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        public async Task<BusinessValidationProxy> Handle(ValidatedNetbsadCommand request, CancellationToken cancellationToken)
        {

            if (request?.Item != null)
            {
                var netbsads = await query.GetListAsync(request.Item.ItemPath, request.Item.ItemId);

                if (netbsads != null && netbsads.Any())
                {
                    var aggregate = new Aggregate<Netbsad>(request.Item, netbsads, null, null)
                    {
                        LongDay = DateTime.ParseExact(BusinessValidationConstants.CONFIG_LONGDAY, BusinessValidationConstants.CONFIG_DATEFORMAT, System.Globalization.CultureInfo.InvariantCulture),
                        ShortDay = DateTime.ParseExact(BusinessValidationConstants.CONFIG_SHORTDAY, BusinessValidationConstants.CONFIG_DATEFORMAT, System.Globalization.CultureInfo.InvariantCulture),
                    };

                    businessValidationProxy = await ValidateNetbsadAsync(aggregate);
                }

            }

            return businessValidationProxy;
        }

        /// <summary>
        /// The ValidateNetbsadAsync
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        private async Task<BusinessValidationProxy> ValidateNetbsadAsync(Aggregate<Netbsad> aggregate)
        {
            inbound = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_INBOUND);
            outbound = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_PROCESSING);
            rejected = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_REJECTED);

            try
            {
                validator.ValidateAndThrow(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);
                var warningResult = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                if (aggregate.ValidFlow.Any())
                {
                    aggregate.ValidFlow.ForEach(e => e.ResetBuySellPriceVolumeAdjustment());

                    await UploadNetBsadRecordsAsSucceeded(aggregate);
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
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <returns>The <see cref="Task"/></returns>       
        private async Task UploadAndPublishRecordsAsRejected(Aggregate<Netbsad> aggregate)
        {
            var uniqueSettlementDateAndPeriods = aggregate.InValidFlow.GroupBy(g => new { g.SettDate, g.SettlementPeriod });

            foreach (var item in uniqueSettlementDateAndPeriods)
            {
                string path = aggregate.Item.ItemPath;
                var outputPath = Common.GetOutputFilePath(item.Select(x => x.SettDate).FirstOrDefault(),
                                    item.Select(x => x.SettlementPeriod).FirstOrDefault(), path, rejected);
                await UploadFileAndStatusForRejected(aggregate, outputPath);
                businessValidationProxy.InValidPaths.Add(outputPath);
            }
            Log.Information(BusinessValidationConstants.NETBSAD_INFO_MSG_INVALIDRECORDUPLOADEDTOREJECTEDFOLDER);
        }

        private async Task UploadFileAndStatusForRejected(Aggregate<Netbsad> aggregate, string outputPath)
        {
            await writer.UpLoadFile(aggregate.InValidFlow, outputPath);
            await writer.UpdateFileStatus(Common.GetStatusOfRejectedFile(outputPath, BusinessValidationConstants.REJECTED));
            businessValidationProxy.InValid = true;
        }


        /// <summary>
        /// The UploadAndPublishRecordsAsSucceeded
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task UploadNetBsadRecordsAsSucceeded(Aggregate<Netbsad> aggregate)
        {

            var uniqueSettlementDateAndPeriods = aggregate.ValidFlow.GroupBy(g => new { g.SettDate, g.SettlementPeriod });

            foreach (var item in uniqueSettlementDateAndPeriods)
            {
                string path = aggregate.Item.ItemPath;
                var outputPath = Common.GetOutputFilePath(item.Select(x => x.SettDate).FirstOrDefault(),
                                    item.Select(x => x.SettlementPeriod).FirstOrDefault(), path, outbound);
                await UploadFileAndStatus(aggregate, outputPath);
                businessValidationProxy.ValidPaths.Add(outputPath);
            }
            Log.Information(BusinessValidationConstants.NETBSAD_INFO_MSG_FILEUPLOADEDTOPROCESSINGFOLDER);
        }

        /// <summary>
        /// The UploadFileAndStatus
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <param name="outputPath">The outputPath<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task UploadFileAndStatus(Aggregate<Netbsad> aggregate, string outputPath)
        {
            await writer.UpLoadFile(aggregate.ValidFlow, outputPath);
            await writer.UpdateFileStatus(Common.GetStatusOfBusinessValidation(outputPath));
            businessValidationProxy.Valid = true;
        }
    }
}
