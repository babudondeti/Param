using Elexon.FA.BusinessValidation.Domain.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Model
{
    public class BoalfTest
    {
        [Fact]
        public async Task Boalf_Should_Return_TypeOf_Boalf()
        {
            await Task.Run(() =>
            {
                Boalf boalf = new Boalf();

                Assert.NotNull(boalf);
                Assert.IsType<Boalf>(boalf);
            });
        }
    }
}
