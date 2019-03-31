namespace Elexon.FA.BusinessValidation.Api.Strategy
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.BusinessValidation.NETBSADFlow.Command;
    using Elexon.FA.Core.IntegrationMessage;
    using Elexon.FA.Core.Logging;
    using MediatR;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="NetbsadStrategy" />
    /// </summary>
    public sealed class NetbsadStrategy : IBusinessValidationStrategy
    {
        /// <summary>
        /// Defines the _mediator
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetbsadStrategy"/> class.
        /// </summary>
        /// <param name="mediator">The mediator<see cref="IMediator"/></param>
        public NetbsadStrategy(IMediator mediator)
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
            ValidatedNetbsadCommand validatedNetbsadCommand = new ValidatedNetbsadCommand(item);
            Log.Information(BusinessValidationConstants.MSG_INFO_BUSINESSVALIDATIONPROCESS_STARTED);
            BusinessValidationProxy businessValidationProxy = await mediator.Send(validatedNetbsadCommand);
            return businessValidationProxy;
        }
    }
}
