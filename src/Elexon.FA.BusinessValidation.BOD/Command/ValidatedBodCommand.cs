namespace Elexon.FA.BusinessValidation.BODFlow.Command
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.Core.IntegrationMessage;
    using MediatR;

    /// <summary>
    /// Defines the <see cref="ValidatedBodCommand" />
    /// </summary>
    public class ValidatedBodCommand : IRequest<BusinessValidationProxy>
    {
        /// <summary>
        /// Gets or sets the Items
        /// </summary>
        public Item Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedBodCommand"/> class.
        /// </summary>
        /// <param name="items">The items<see cref="Item"/></param>
        public ValidatedBodCommand(Item items)
        {
            Items = items;
        }
    }
}
