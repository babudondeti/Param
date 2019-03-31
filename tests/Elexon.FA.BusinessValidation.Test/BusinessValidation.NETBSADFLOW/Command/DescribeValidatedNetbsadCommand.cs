using Elexon.FA.BusinessValidation.NETBSADFlow.Command;
using Elexon.FA.Core.IntegrationMessage;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.NETBSADFLOW.Command
{
    public class DescribeValidatedNetbsadCommand
    {
        [Fact]
        public async Task ItShouldBeValidType()
        {
            await Task.Run(() =>
            {
                Item items = new Item();
                ValidatedNetbsadCommand result = new ValidatedNetbsadCommand(items);
                Assert.IsType<ValidatedNetbsadCommand>(result);
            });

        }
    }
}
