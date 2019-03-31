using Elexon.FA.BusinessValidation.Api.Configurations;
using Elexon.FA.Core.Blob;
using Elexon.FA.Core.Blob.Abstractions;
using Elexon.FA.Core.KeyVault.Abstractions;
using Elexon.FA.Core.ServiceBus.Abstractions;
using Elexon.FA.Core.ServiceBus.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Configurations
{
    public class DependencyConfigurationExtensionTest
    {
        readonly IServiceCollection _service;
        readonly ICoreKeyVaultClient _keyVaultClient;

        public DependencyConfigurationExtensionTest()
        {
            _service = new ServiceCollection();
            _keyVaultClient = new MockCoreKeyVaultClients();
        }
        [Fact]
        public async Task Should_ExecuteRegisterBlobwithoutException()
        {
            await Task.Run(() =>
            {
                _service.RegisterBlob(_keyVaultClient);
                ServiceProvider builder = _service.BuildServiceProvider();
                var storage = builder.GetService<ICoreBlobStorage>();
                Assert.NotNull(storage);
                Assert.IsType<CoreBlobStorage>(storage);
            });
        }
        [Fact]
        public async Task Should_ExecuteRegisterServiceBuswithoutException()
        {
            await Task.Run(() =>
            {
                _service.RegisterServiceBus(_keyVaultClient);
                ServiceProvider builder = _service.BuildServiceProvider();
                var storage = builder.GetService<IReceiveManager>();
                Assert.NotNull(storage);
                Assert.IsType<ReceiveManager>(storage);
            });
        }
        [Fact]
        public async Task Should_ExecuteRegisterLogswithoutException()
        {
            await Task.Run(() =>
            {
                _service.RegisterLogs(_keyVaultClient);      
            });
        }
    }

    public class MockCoreKeyVaultClients : ICoreKeyVaultClient
    {
        public Task<string> GetSecret(string keyName)
        {
            string value = "";
            var result = Task.FromResult<string>(value);
            if (keyName == "elxkvvStorageLrsh02Name")
                result = Task.FromResult<string>("testelxkvvStorageLrsh02Name");
            else if (keyName == "elxkvvStorageLrsh02Key1")
                result = Task.FromResult<string>("testelxkvvStorageLrsh02Key1");
            else if (keyName == "elxkvvblobcontainername01")
                result = Task.FromResult<string>("testelxkvvblobcontainername01");
            else if (keyName == "elxkvvServiceBusConnectionString01")
                result = Task.FromResult<string>("testelxkvvServiceBusConnectionString01");
            else if (keyName == "elxkvvgenericfilevalidationtopicpub")
                result = Task.FromResult<string>("testelxkvvgenericfilevalidationtopicpub");
            else if (keyName == "elxkvvbusinessvalidationcompletedsubscription")
                result = Task.FromResult<string>("testelxkvvbusinessvalidationcompletedsubscription");
            else if (keyName == "elxkvvapplicationInsightsInstrumentationkey01")
                result = Task.FromResult<string>("testelxkvvapplicationInsightsInstrumentationkey01");
            return result;
        }
    }
}
