using Elexon.FA.BusinessValidation.Domain.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Model
{
    public class NetbsadTest
    {
        [Fact]
        public async Task Netbsad_Should_Return_TypeOf_Netbsad()
        {
            await Task.Run(() =>
            {

                Netbsad netbsad = new Netbsad();

                Assert.NotNull(netbsad);
                Assert.IsType<Netbsad>(netbsad);
            });
                
        }
    }
}
