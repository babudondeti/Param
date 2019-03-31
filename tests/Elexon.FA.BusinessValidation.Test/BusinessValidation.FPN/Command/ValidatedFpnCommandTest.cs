using Elexon.FA.BusinessValidation.FPNFlow.Command;
using Elexon.FA.Core.IntegrationMessage;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.FPN.Command
{
    public class ValidatedFpnCommandTest
    {
        [Fact]
        public async Task ValidatedFpnCommand_Command_Should_ValidatedFpnCommand_Type()
        {
            await Task.Run(() =>
            {

                Item items = new Item();
                ValidatedFpnCommand result = new ValidatedFpnCommand(items);
                Assert.IsType<ValidatedFpnCommand>(result);
            });
        }
    }
}
