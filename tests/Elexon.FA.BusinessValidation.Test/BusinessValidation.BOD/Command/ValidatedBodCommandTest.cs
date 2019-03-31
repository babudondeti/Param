using Elexon.FA.BusinessValidation.BODFlow.Command;
using Elexon.FA.Core.IntegrationMessage;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD.Command
{
    public class ValidatedBodCommandTest
    {
        [Fact]
        public async Task ValidatedBodCommand_Command_Should_ValidatedBodCommand_Type()
        {
            await Task.Run(() =>
            {

                Item items = new Item();
                ValidatedBodCommand result = new ValidatedBodCommand(items);
                Assert.IsType<ValidatedBodCommand>(result);
            });
        }
    }
}
