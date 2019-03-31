namespace Elexon.FA.BusinessValidation.FPNFlow.Command
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
    using Elexon.FA.BusinessValidation.Domain.Query;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.BusinessValidation.FPNFlow.Validator;
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
    /// Defines the <see cref="ValidatedFpnCommandHandler" />
    /// </summary>
    public class ValidatedFpnCommandHandler : IRequestHandler<ValidatedFpnCommand, BusinessValidationProxy>
    {
        /// <summary>
        /// Defines the _coreKeyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient _coreKeyVaultClient;

        /// <summary>
        /// Defines the _query
        /// </summary>
        private readonly IQueryFlow<Fpn> _query;

        /// <summary>
        /// Defines the _writer
        /// </summary>
        private readonly IProjectionWriter _writer;

        /// <summary>
        /// Defines the _validator
        /// </summary>
        private readonly FpnValidator _validator = new FpnValidator();

        /// <summary>
        /// Defines the businessValidationProxy
        /// </summary>
        internal BusinessValidationProxy businessValidationProxy = new BusinessValidationProxy();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedFpnCommandHandler"/> class.
        /// </summary>
        /// <param name="query">The query<see cref="IQueryFlow{Fpn}"/></param>
        /// <param name="writer">The writer<see cref="IProjectionWriter"/></param>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public ValidatedFpnCommandHandler(IQueryFlow<Fpn> query, IProjectionWriter writer, IApplicationBuilder applicationBuilder)
        {
            _query = query;
            _writer = writer;
            _coreKeyVaultClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
        }

        /// <summary>
        /// The Handle
        /// </summary>
        /// <param name="request">The request<see cref="ValidatedFpnCommand"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        public async Task<BusinessValidationProxy> Handle(ValidatedFpnCommand request, CancellationToken cancellationToken)
        {
            if (request?.Items != null)
            {
                var fpnList = await _query.GetListAsync(request.Items.ItemPath, request.Items.ItemId);
                var bmuParticipationList = await _query.GetBmuParticipationAsync(fpnList.FirstOrDefault().TimeFrom, fpnList.FirstOrDefault().TimeTo);
                var aggregate = new Aggregate<Fpn>(request.Items, fpnList, bmuParticipationList, null);
                if (aggregate.BusinessValidationFlow.Count > 0)
                {
                    await GetConfigurationDataFromKeyVault(aggregate);
                    businessValidationProxy = await FpnValidationProcessAsync(aggregate);
                }
            }
            return businessValidationProxy;
        }

        /// <summary>
        /// The GetConfigurationDataFromKeyVault
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        private async Task GetConfigurationDataFromKeyVault(Aggregate<Fpn> aggregate)
        {
            var settlementDuration = await _coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_SETTLEMENTDURATION);
            aggregate.SettlementDuration = int.Parse(settlementDuration);
        }

        /// <summary>
        /// The FpnValidationProcessAsync
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        private async Task<BusinessValidationProxy> FpnValidationProcessAsync(Aggregate<Fpn> aggregate)
        {
            string outbound = await _coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_PROCESSING);
            string rejected = await _coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_REJECTED);
            var path = aggregate.Item.ItemPath;
            path = Common.GetOutputFolderPath(aggregate.BusinessValidationFlow.Select(x => x.SettlementDay).FirstOrDefault(), 
                        Convert.ToInt32(aggregate.BusinessValidationFlow.Select(x => x.SettlementPeriod).FirstOrDefault()), path, outbound);
            var fileExists = _query.ExistsAsync(path);
            aggregate.FileAlreadyExistOrNot = await fileExists;
            ValidationResult validationResult = await ErrorCheck(aggregate);
            if (validationResult.IsValid)
            {
                ValidationResult warningResult = await WarningCheck(aggregate);
                if (aggregate.ValidFlow.Count > 0)
                {
                    await FileProcessAsync(aggregate.ValidFlow, aggregate.Item.ItemPath, true, warningResult, outbound);
                    Log.Information(BusinessValidationConstants.MSG_FPNUPLOADEDTOPROCESSING);
                    businessValidationProxy.ValidationResult = warningResult.IsValid;
                }
                if (aggregate.InValidFlow.Count > 0)
                {
                    await FileProcessAsync(aggregate.InValidFlow, aggregate.Item.ItemPath, false, warningResult, rejected);
                    Log.Information(BusinessValidationConstants.MSG_FPNRECORDUPLOADEDTOREJECTED);
                }

            }
            else
            {
                await FileProcessAsync(aggregate.BusinessValidationFlow, aggregate.Item.ItemPath, false, validationResult, rejected);
                Log.Information(BusinessValidationConstants.MSG_FPNFILEUPLOADEDTOREJECTED);
            }

            return businessValidationProxy;
        }

        /// <summary>
        /// The WarningCheck
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="Task{ValidationResult}"/></returns>
        private async Task<ValidationResult> WarningCheck(Aggregate<Fpn> aggregate)
        {
            var warningResult = _validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
            string warningCheck = BusinessValidationConstants.WARNINGCHECK;
            await ExceptionCatch(aggregate, warningCheck);
            return warningResult;
        }

        /// <summary>
        /// The ErrorCheck
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="Task{ValidationResult}"/></returns>
        private async Task<ValidationResult> ErrorCheck(Aggregate<Fpn> aggregate)
        {
            var validationResult = _validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);
            string errorCheck = BusinessValidationConstants.ERRORCHECK;
            await ExceptionCatch(aggregate, errorCheck);
            return validationResult;
        }

        /// <summary>
        /// The ExceptionCatch
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <param name="errorORwarningCheck">The errorORwarningCheck<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task ExceptionCatch(Aggregate<Fpn> aggregate, string errorORwarningCheck)
        {
            try
            {
                _validator.ValidateAndThrow(aggregate, ruleSet: errorORwarningCheck);
            }
            catch (Exception ex)
            {
                await LogException(ex, aggregate);
            }
        }

        /// <summary>
        /// The FileProcessAsync
        /// </summary>
        /// <param name="fpns">The fpns<see cref="List{Fpn}"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <param name="isValid">The isValid<see cref="bool"/></param>
        /// <param name="warningResult">The warningResult<see cref="ValidationResult"/></param>
        /// <param name="folderName">The folderName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task FileProcessAsync(List<Fpn> fpns, string path, bool isValid, ValidationResult warningResult, string folderName)
        {
            path = Common.GetOutputFolderPath(fpns.Select(x => x.SettlementDay).FirstOrDefault(), 
                Convert.ToInt32(fpns.Select(x => x.SettlementPeriod).FirstOrDefault()), path, folderName);
            await _writer.UpLoadFile(fpns, path);
            if (isValid)
            {
                businessValidationProxy.Valid = true;
                businessValidationProxy.ValidPaths.Add(path);
                businessValidationProxy.ValidationResult = warningResult.IsValid;
                await _writer.UpdateFileStatus(Common.GetStatusOfBusinessValidation(path));
            }
            else
            {
                businessValidationProxy.InValid = true;
                businessValidationProxy.InValidPaths.Add(path);
                await _writer.UpdateFileStatus(Common.GetStatusOfRejectedFile(path, BusinessValidationConstants.WARNING));
            }
        }

        /// <summary>
        /// The LogException
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task LogException(Exception ex, Aggregate<Fpn> aggregate)
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
