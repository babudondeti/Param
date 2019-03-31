using Elexon.FA.BusinessValidation.BOALFFlow.Command;
using Elexon.FA.Core.IntegrationMessage;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF.Command
{
    public class ValidatedBoalfCommandTest
    {
        [Fact]
        public async Task ValidatedBoalfCommand_Command_Should_ValidatedBoalfCommand_Type()
        {

            await Task.Run(() =>
            {
                Item items = new Item();
                ValidatedBoalfCommand result = new ValidatedBoalfCommand(items);
                Assert.IsType<ValidatedBoalfCommand>(result);
            });
        }
    }
}
