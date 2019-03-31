using Elexon.FA.BusinessValidation.DISBSADFlow.Command;
using Elexon.FA.Core.IntegrationMessage;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.DISBSADFLOW.Command
{
    public class DescribeValidatedDisbsadCommand
    {
        [Fact]
        public async Task ItShouldBeValidType()
        {
            await Task.Run(() =>
            {
                Item item = new Item();
                ValidatedDisbsadCommand result = new ValidatedDisbsadCommand(item);
                Assert.IsType<ValidatedDisbsadCommand>(result);
            });

        }
    }
}
