using Elexon.FA.BusinessValidation.Domain.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Model
{
    public class CreationTimeTest
    {
        [Fact]

        public async Task Should_Check_For_NotNull_TypeOf_CreationTime()
        {
            await Task.Run(() =>
            {
                CreationTime creationTime = new CreationTime();

                Assert.NotNull(creationTime);
                Assert.IsType<CreationTime>(creationTime);
            });
        }
    }
}
