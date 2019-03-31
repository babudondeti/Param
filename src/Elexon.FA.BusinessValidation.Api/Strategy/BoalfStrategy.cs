namespace Elexon.FA.BusinessValidation.Api.Strategy
{
    using Elexon.FA.BusinessValidation.BOALFFlow.Command;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.Exceptions;
    using Elexon.FA.Core.IntegrationMessage;
    using Elexon.FA.Core.Logging;
    using MediatR;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="BoalfStrategy" />
    /// </summary>
    public sealed class BoalfStrategy : IBusinessValidationStrategy
    {
        /// <summary>
        /// Defines the _mediator
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoalfStrategy"/> class.
        /// </summary>
        /// <param name="mediator">The mediator<see cref="IMediator"/></param>
        public BoalfStrategy(IMediator mediator)
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
            Guard.AgainstNull(nameof(item), item);
            ValidatedBoalfCommand validatedBoalfCommand = new ValidatedBoalfCommand(item);
            Log.Information(BusinessValidationConstants.MSG_BUSINESSVALIDATIONSTARTED);
            BusinessValidationProxy businessValidationProxy = await mediator.Send(validatedBoalfCommand);

            return businessValidationProxy;
        }
    }
}
