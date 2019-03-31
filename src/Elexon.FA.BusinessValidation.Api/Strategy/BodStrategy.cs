namespace Elexon.FA.BusinessValidation.Api.Strategy
{
    using Elexon.FA.BusinessValidation.BODFlow.Command;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.IntegrationMessage;
    using Elexon.FA.Core.Logging;
    using MediatR;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="BodStrategy" />
    /// </summary>
    public sealed class BodStrategy : IBusinessValidationStrategy
    {
        /// <summary>
        /// Defines the _mediator
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodStrategy"/> class.
        /// </summary>
        /// <param name="mediator">The mediator<see cref="IMediator"/></param>
        public BodStrategy(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// The ExecuteStrategy
        /// </summary>
        /// <param name="FlowName">The FlowName<see cref="string"/></param>
        /// <param name="item">The item<see cref="Item"/></param>
        /// <returns>The <see cref="Task{BusinessValidationProxy}"/></returns>
        async Task<BusinessValidationProxy> IBusinessValidationStrategy.ExecuteStrategy(string FlowName, Item item)
        {
            ValidatedBodCommand validatedBodCommand = new ValidatedBodCommand(item);
            Log.Information(BusinessValidationConstants.MSG_BUSINESSVALIDATIONSTARTED);
            BusinessValidationProxy businessValidationProxy = await mediator.Send(validatedBodCommand);
            return businessValidationProxy;
        }
    }
}
