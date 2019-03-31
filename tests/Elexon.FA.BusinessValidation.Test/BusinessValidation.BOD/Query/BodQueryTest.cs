using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.Core.Blob.Abstractions;
using Elexon.FA.Core.HttpClient;
using Elexon.FA.Core.KeyVault.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD.Query
{
    public class BodQueryTest : IDisposable
    {
        private Mock<IApplicationBuilder> mockApplicationBuilder;
        readonly IServiceCollection service;
        private readonly Mock<ICoreHttpServiceClient> mockCoreHttpServiceClient = new Mock<ICoreHttpServiceClient>();
        private readonly Mock<ICoreKeyVaultClient> mockCoreKeyVaultClient = new Mock<ICoreKeyVaultClient>();

        public BodQueryTest()
        {
            service = new ServiceCollection();
            service.AddSingleton<ICoreBlobStorage, MockCoreBlobStorage>();
            service.AddSingleton<ICoreHttpServiceClient, MockCoreHttpServiceClient>();
            service.AddSingleton<ICoreKeyVaultClient, MockCoreKeyVaultClient>();
            mockApplicationBuilder = new Mock<IApplicationBuilder>();
            mockApplicationBuilder.SetupGet(a => a.ApplicationServices).Returns(service.BuildServiceProvider());
        }
        /// <summary>
        /// Dispose unused resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// The Clean up
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            mockApplicationBuilder = null;
        }  

        [Fact]
        public async Task BodQuery_GetBodListAsync_Should_Return_Value_When_There_Is_No_Exception()
        {

            await Task.Run(() =>
            {
                var query = new QueryFlow<Bod>(mockApplicationBuilder.Object);
                var result = query.GetListAsync("", "BOD").Result;

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
            });
        }


    }
}
