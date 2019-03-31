namespace Elexon.FA.BusinessValidation.FPNFlow.Command
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.Core.IntegrationMessage;
    using MediatR;

    /// <summary>
    /// Defines the <see cref="ValidatedFpnCommand" />
    /// </summary>
    public class ValidatedFpnCommand : IRequest<BusinessValidationProxy>
    {
        /// <summary>
        /// Gets or sets the Items
        /// </summary>
        public Item Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedFpnCommand"/> class.
        /// </summary>
        /// <param name="items">The items<see cref="Item"/></param>
        public ValidatedFpnCommand(Item items)
        {
            Items = items;
        }
    }
}
