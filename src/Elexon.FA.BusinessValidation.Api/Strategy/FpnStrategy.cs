namespace Elexon.FA.BusinessValidation.Api.Strategy
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.BusinessValidation.FPNFlow.Command;
    using Elexon.FA.Core.IntegrationMessage;
    using Elexon.FA.Core.Logging;
    using MediatR;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="FpnStrategy" />
    /// </summary>
    public sealed class FpnStrategy : IBusinessValidationStrategy
    {
        /// <summary>
        /// Defines the _mediator
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FpnStrategy"/> class.
        /// </summary>
        /// <param name="mediator">The mediator<see cref="IMediator"/></param>
        public FpnStrategy(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// The ExecuteStrategy
        /// </summary>
        /// <param name="FlowName">The FlowName<see cref="string"/></param>
        /// <param name="item">The item<see cref="Item"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        public async Task<BusinessValidationProxy> ExecuteStrategy(string FlowName, Item item)
        {
            ValidatedFpnCommand validatedFpnCommand = new ValidatedFpnCommand(item);
            Log.Information(BusinessValidationConstants.MSG_BUSINESSVALIDATIONSTARTED);
            BusinessValidationProxy businessValidationProxy = await _mediator.Send(validatedFpnCommand);

            return businessValidationProxy;
        }
    }
}
