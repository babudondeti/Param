namespace Elexon.FA.BusinessValidation.BOALFFlow.Command
{
    using Elexon.FA.BusinessValidation.BOALFFlow.Validator;
    using Elexon.FA.BusinessValidation.BOALFlow.FileProcess;
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Query;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.KeyVault.Abstractions;
    using Elexon.FA.Core.Logging;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ValidatedBoalfCommandHandler" />
    /// </summary>
    public class ValidatedBoalfCommandHandler : IRequestHandler<ValidatedBoalfCommand, BusinessValidationProxy>
    {
        /// <summary>
        /// Defines the _coreKeyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient coreKeyVaultClient;

        /// <summary>
        /// Defines the _query
        /// </summary>
        private readonly IQueryFlow<Boalf> query;

        /// <summary>
        /// Defines the _validator
        /// </summary>
        private readonly BoalfValidator validator = new BoalfValidator();

        /// <summary>
        /// Defines the _fileProcessService
        /// </summary>
        private readonly IFileProcessService fileProcessService;

        /// <summary>
        /// Defines the businessValidationProxy
        /// </summary>
        internal BusinessValidationProxy businessValidationProxy = new BusinessValidationProxy();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedBoalfCommandHandler"/> class.
        /// </summary>
        /// <param name="query">The query<see cref="IQueryFlow{Boalf}"/></param>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        /// <param name="fileProcessService">The fileProcessService<see cref="IFileProcessService"/></param>
        public ValidatedBoalfCommandHandler(IQueryFlow<Boalf> query, IApplicationBuilder applicationBuilder, IFileProcessService fileProcessService)
        {
            this.query = query;
            if(applicationBuilder!= null)
            {
                coreKeyVaultClient = applicationBuilder.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
            }

            this.fileProcessService = fileProcessService;
        }

        /// <summary>
        /// The Handle
        /// </summary>
        /// <param name="request">The request<see cref="ValidatedBoalfCommand"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        public async Task<BusinessValidationProxy> Handle(ValidatedBoalfCommand request, CancellationToken cancellationToken)
        {
            if (request!= null && request.Items != null)
            {
                var boalfList = await query.GetListAsync(request.Items.ItemPath, request.Items.ItemId);
                var boalfGroups = boalfList.GroupBy(g => new { g.BmuName, g.BidOfferAcceptanceNumber, g.AcceptanceTime });
                List<ParticipantEnergyAsset> participantEnergyAsset = new List<ParticipantEnergyAsset>();
                List<BoalfIndexTable> boalfIndexTables = new List<BoalfIndexTable>();
                foreach (var boalfGroup in boalfGroups)
                {
                    participantEnergyAsset.AddRange(await query.GetBmuParticipationAsync(boalfGroup.FirstOrDefault().TimeFrom, boalfGroup.FirstOrDefault().TimeTo));
                    boalfIndexTables.AddRange(await query.GetListBoalfIndexTable(boalfGroup.FirstOrDefault().BmuName, boalfGroup.FirstOrDefault().BidOfferAcceptanceNumber.ToString()
                        , boalfGroup.FirstOrDefault().AcceptanceTime.ToString("yyyy-MM-dd HH:mm")));
                }
                var aggregate = new Aggregate<Boalf>(request.Items, boalfList, participantEnergyAsset, boalfIndexTables);
                if (aggregate.BusinessValidationFlow.Count > 0)
                {
                    await GetConfigurationDataFromKeyVault(aggregate);
                    businessValidationProxy = await BoalfValidationProcessAsync(aggregate);
                }
            }
            return businessValidationProxy;
        }

        /// <summary>
        /// The GetConfigurationDataFromKeyVault
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        private async Task GetConfigurationDataFromKeyVault(Aggregate<Boalf> aggregate)
        {
            try
            {
                var settlementDuration =await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_SETTLEMENTDURATION);
                aggregate.SettlementDuration = int.Parse(settlementDuration);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// The BoalfValidationProcessAsync
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        private async Task<BusinessValidationProxy> BoalfValidationProcessAsync(Aggregate<Boalf> aggregate)
        {
            WarningCheck(aggregate);
            try
            {
                await fileProcessService.FileProcess(aggregate);
                if (aggregate.ValidFlow.Count > 0)
                {
                    businessValidationProxy.Valid = true;
                    businessValidationProxy.ValidPaths.AddRange(aggregate.ValidPath);
                    Log.Information(BusinessValidationConstants.BOALF_MSG_SUCCESSMESSAGE);
                }
                if (aggregate.InValidFlow.Count > 0)
                {
                    businessValidationProxy.InValid = true;
                    businessValidationProxy.InValidPaths.AddRange(aggregate.InValidPath);
                    Log.Information(BusinessValidationConstants.BOALF_MSG_FAILUREMESSAGE);
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

        private void WarningCheck(Aggregate<Boalf> aggregate)
        {
            try
            {
                validator.ValidateAndThrow(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                aggregate.ErrorMessage = ex.Message;
                businessValidationProxy.ErrorMessages.Add(ex.Message);
            }
        }
    }
}
