namespace Elexon.FA.BusinessValidation.BOALFFlow.Command
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.Core.IntegrationMessage;
    using MediatR;

    /// <summary>
    /// Defines the <see cref="ValidatedBoalfCommand" />
    /// </summary>
    public class ValidatedBoalfCommand : IRequest<BusinessValidationProxy>
    {
        /// <summary>
        /// Gets or sets the Items
        /// </summary>
        public Item Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedBoalfCommand"/> class.
        /// </summary>
        /// <param name="items">The items<see cref="Item"/></param>
        public ValidatedBoalfCommand(Item items)
        {
            Items = items;
        }
    }
}
