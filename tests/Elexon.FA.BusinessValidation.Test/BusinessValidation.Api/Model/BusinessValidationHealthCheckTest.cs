using Elexon.FA.BusinessValidation.Api.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Model
{
    public class BusinessValidationHealthCheckTest
    {
        [Fact]
        public async Task BusinessValidationHealthCheck_Should_Return_TypeOf_BusinessValidationHealthCheck()
        {
            await Task.Run(() =>
            {
                BusinessValidationHealthCheck businessValidationHealthCheck = new BusinessValidationHealthCheck();
                Assert.NotNull(businessValidationHealthCheck);
                Assert.IsType<BusinessValidationHealthCheck>(businessValidationHealthCheck);
            });
        }
    }
}
