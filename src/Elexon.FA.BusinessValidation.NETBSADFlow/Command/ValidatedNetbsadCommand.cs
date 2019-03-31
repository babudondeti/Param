namespace Elexon.FA.BusinessValidation.NETBSADFlow.Command
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.Core.IntegrationMessage;
    using MediatR;

    /// <summary>
    /// Defines the <see cref="ValidatedNetbsadCommand" />
    /// </summary>
    public class ValidatedNetbsadCommand : IRequest<BusinessValidationProxy>
    {
        /// <summary>
        /// Gets or sets the Item
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedNetbsadCommand"/> class.
        /// </summary>
        /// <param name="item">The item<see cref="Item"/></param>
        public ValidatedNetbsadCommand(Item item)
        {
            Item = item;
        }
    }
}
