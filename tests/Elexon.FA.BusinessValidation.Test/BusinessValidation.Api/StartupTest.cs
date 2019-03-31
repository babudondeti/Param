using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api
{
    public class StartupTest
    {
        private readonly Mock<IConfigurationSection> configurationSectionStub;
        private readonly Mock<IConfiguration> configurationStub;
        private readonly IServiceCollection services = new ServiceCollection();
       
        public StartupTest()
        {
            configurationSectionStub = new Mock<IConfigurationSection>();
            configurationStub = new Mock<IConfiguration>();
        }

    }
}
