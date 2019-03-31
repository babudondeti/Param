namespace Elexon.FA.BusinessValidation.BODFlow.Command
{
    using Elexon.FA.BusinessValidation.BODFlow.Validator;
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
    using Elexon.FA.BusinessValidation.Domain.Query;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.KeyVault.Abstractions;
    using Elexon.FA.Core.Logging;
    using FluentValidation;
    using FluentValidation.Results;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ValidatedBodCommandHandler" />
    /// </summary>
    public class ValidatedBodCommandHandler : IRequestHandler<ValidatedBodCommand, BusinessValidationProxy>
    {
        /// <summary>
        /// Defines the _coreKeyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient coreKeyVaultClient;

        /// <summary>
        /// Defines the _query
        /// </summary>
        private readonly IQueryFlow<Bod> query;

        /// <summary>
        /// Defines the _writer
        /// </summary>
        private readonly IProjectionWriter writer;

        /// <summary>
        /// Defines the _validator
        /// </summary>
        private readonly BodValidator validator = new BodValidator();

        /// <summary>
        /// Defines the businessValidationProxy
        /// </summary>
        internal BusinessValidationProxy businessValidationProxy = new BusinessValidationProxy();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedBodCommandHandler"/> class.
        /// </summary>
        /// <param name="query">The query<see cref="IQueryFlow{Bod}"/></param>
        /// <param name="writer">The writer<see cref="IProjectionWriter"/></param>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public ValidatedBodCommandHandler(IQueryFlow<Bod> query, IProjectionWriter writer, IApplicationBuilder applicationBuilder)
        {
            this.query = query;
            this.writer = writer;
            coreKeyVaultClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
        }

        /// <summary>
        /// The Handle
        /// </summary>
        /// <param name="request">The request<see cref="ValidatedBodCommand"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        public async Task<BusinessValidationProxy> Handle(ValidatedBodCommand request, CancellationToken cancellationToken)
        {
            if (request!=null && request.Items != null)
            {
                var bodList = await query.GetListAsync(request.Items.ItemPath, request.Items.ItemId);
                var bmuParticipationList = await query.GetBmuParticipationAsync(bodList.FirstOrDefault().TimeFrom, bodList.FirstOrDefault().TimeTo);
                var aggregate = new Aggregate<Bod>(request.Items, bodList, bmuParticipationList, null);
                if (aggregate.BusinessValidationFlow.Count > 0)
                {
                   await  GetConfigurationDataFromKeyVault(aggregate);
                    businessValidationProxy = await BodValidationProcessAsync(aggregate);
                }
            }
            return businessValidationProxy;
        }

        /// <summary>
        /// The GetConfigurationDataFromKeyVault
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        private async Task GetConfigurationDataFromKeyVault(Aggregate<Bod> aggregate)
        {
            var settlementDuration =await  coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_SETTLEMENTDURATION);
            var minPairId = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_MINPAIRID);
            var maxPairId = await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_MAXPAIRID);
            aggregate.MaxPairId = int.Parse(maxPairId);
            aggregate.MinPairId = int.Parse(minPairId);
            aggregate.SettlementDuration = int.Parse(settlementDuration);
        }

        /// <summary>
        /// The BodValidationProcessAsync
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        private async Task<BusinessValidationProxy> BodValidationProcessAsync(Aggregate<Bod> aggregate)
        {
            string outbound =await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_PROCESSING);
            string rejected =await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_REJECTED);
            ValidationResult validationResult = await ErrorCheck(aggregate);
            if (validationResult.IsValid)
            {
                ValidationResult warningResult = await WarningCheck(aggregate);
                if (aggregate.ValidFlow.Count > 0)
                {
                    await FileProcessAsync(aggregate.ValidFlow, aggregate.Item.ItemPath, true, warningResult, outbound);
                    Log.Information(BusinessValidationConstants.MSG_BODUPLOADEDTOPROCESSING);

                }
                if (aggregate.InValidFlow.Count > 0)
                {
                    await FileProcessAsync(aggregate.InValidFlow, aggregate.Item.ItemPath, false, warningResult, rejected);
                    Log.Information(BusinessValidationConstants.MSG_BODRECORDUPLOADEDTOREJECTED);
                }
            }
            else
            {
                await FileProcessAsync(aggregate.BusinessValidationFlow, aggregate.Item.ItemPath, false, validationResult, rejected);
                Log.Information(BusinessValidationConstants.MSG_BODFILEUPLOADEDTOREJECTED);
            }
            return businessValidationProxy;
        }

        /// <summary>
        /// The WarningCheck
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="Task{ValidationResult}"/></returns>
        private async Task<ValidationResult> WarningCheck(Aggregate<Bod> aggregate)
        {
            var warningResult = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
            var warningCheck = BusinessValidationConstants.WARNINGCHECK;
            await ExceptionCatch(aggregate, warningCheck);
            return warningResult;
        }

        /// <summary>
        /// The ErrorCheck
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="Task{ValidationResult}"/></returns>
        private async Task<ValidationResult> ErrorCheck(Aggregate<Bod> aggregate)
        {
            var validationResult = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);
            var  errorCheck = BusinessValidationConstants.ERRORCHECK;
            await ExceptionCatch(aggregate, errorCheck);
            return validationResult;
        }

        /// <summary>
        /// The ExceptionCatch
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <param name="errorORwarningCheck">The errorORwarningCheck<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExceptionCatch(Aggregate<Bod> aggregate, string errorORwarningCheck)
        {
            try
            {
                validator.ValidateAndThrow(aggregate, ruleSet: errorORwarningCheck);
            }
            catch (Exception ex)
            {
                await LogException(ex, aggregate);
            }
        }

        /// <summary>
        /// The LogErrorMessage
        /// </summary>
        /// <param name="result">The result<see cref="ValidationResult"/></param>
        private static void LogErrorMessage(ValidationResult result)
        {
            var error = result.Errors;
            for (int e = 0; e < error.Count; e++)
            {
                Log.Error(error[e].ErrorMessage);
            }
        }

        /// <summary>
        /// The FileProcessAsync
        /// </summary>
        /// <param name="bods">The bods<see cref="List{Bod}"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <param name="isValid">The isValid<see cref="bool"/></param>
        /// <param name="warningResult">The warningResult<see cref="ValidationResult"/></param>
        /// <param name="folderName">The folderName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task FileProcessAsync(List<Bod> bods, string path, bool isValid, ValidationResult warningResult, string folderName)
        {
            path = Common.GetOutputFolderPath(bods.Select(x => x.SettlementDay).FirstOrDefault(), 
                        Convert.ToInt32(bods.Select(x => x.SettlementPeriod).FirstOrDefault()), path, folderName);
            await writer.UpLoadFile(bods, path);
            if (isValid)
            {
                businessValidationProxy.Valid = true;
                businessValidationProxy.ValidPaths.Add(path);
                businessValidationProxy.ValidationResult = warningResult.IsValid;
            }
            else
            {
                businessValidationProxy.InValid = true;
                businessValidationProxy.InValidPaths.Add(path);
                LogErrorMessage(warningResult);
            }
        }

        /// <summary>
        /// The LogException
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task LogException(Exception ex, Aggregate<Bod> aggregate)
        {
            await Task.Run(() =>
            {
                Log.Error(ex.Message);
                aggregate.ErrorMessage = ex.Message;
                businessValidationProxy.ErrorMessages.Add(ex.Message);
            });
        }
    }
}
