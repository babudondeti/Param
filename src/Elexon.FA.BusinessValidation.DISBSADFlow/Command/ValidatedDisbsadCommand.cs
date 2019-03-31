using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.Core.IntegrationMessage;
using MediatR;

namespace Elexon.FA.BusinessValidation.DISBSADFlow.Command
{
    public class ValidatedDisbsadCommand : IRequest<BusinessValidationProxy>
    {
        public Item Item { get; set; }

        public ValidatedDisbsadCommand(Item item)
        {
            Item = item;
        }

    }
}
