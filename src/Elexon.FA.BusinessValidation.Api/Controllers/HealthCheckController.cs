namespace Elexon.FA.BusinessValidation.Api.Controllers
{
    using Elexon.FA.Core.Controllers.Abstractions;
    using Elexon.FA.Core.Healthcheck;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    /// <summary>

    /// Defines the <see cref="HealthCheckController" />
    /// </summary>
    [Route("hc1")]
    public class HealthCheckController : CoreController
    {
        /// <summary>
        /// Defines the _healthCheckServices
        /// </summary>
        internal readonly IHealthCheckServices healthCheckServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckController"/> class.
        /// </summary>
        /// <param name="healthCheckServices">The healthCheckServices<see cref="IHealthCheckServices"/></param>
        public HealthCheckController(IHealthCheckServices healthCheckServices)
        {
            this.healthCheckServices = healthCheckServices;
        }

        /// <summary>
        /// The Get
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await healthCheckServices.GetHealthCheck();
            return Ok(result);
        }
    }
}
