using Elexon.FA.BusinessValidation.Api.Controllers;
using Elexon.FA.Core.Healthcheck;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Controller
{
    public class HealthCheckControllerTest
    {
        private readonly Mock<IHealthCheckServices> _healthMockCheckServices;
        public HealthCheckControllerTest()
        {
            _healthMockCheckServices = new Mock<IHealthCheckServices>();
        }
        [Fact]
        public async Task Should_Check_TypeOf_Get_Status_Of_HttpRequest()
        {
            HealthCheckController healthCheckTest = new HealthCheckController(_healthMockCheckServices.Object);
            var actionResult = await healthCheckTest.Get();
            var resultStatus = Assert.IsType<OkObjectResult>(actionResult);
            if (resultStatus.StatusCode == 200)
                Assert.Equal(200, resultStatus.StatusCode);
        }
    }
}
