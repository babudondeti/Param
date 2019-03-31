using Elexon.FA.BusinessValidation.Domain.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Model
{
    public class DisbsadTest
    {
        [Fact]
        public async Task Disbsad_Should_Return_TypeOf_Disbsad()
        {
            await Task.Run(() =>
            {
                Disbsad disbsad = new Disbsad();

                Assert.NotNull(disbsad);
                Assert.IsType<Disbsad>(disbsad);
            });
        }
    }
}
