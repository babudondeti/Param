using Elexon.FA.BusinessValidation.Domain.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Model
{
    public class BodTest
    {
        [Fact]
        public async Task Bod_Should_Return_TypeOf_Bod()
        {
            await Task.Run(() =>
            {
                Bod bod = new Bod();

                Assert.NotNull(bod);
                Assert.IsType<Bod>(bod);
            });
        }
    }
}
