using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD;
using Elexon.FA.Core.Blob.Abstractions;
using Elexon.FA.Core.HttpClient;
using Elexon.FA.Core.KeyVault.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.NETBSADFLOW.Query
{
    public class DescribeNetbsadQuery : IDisposable
    {
        private Mock<IApplicationBuilder> mockApplicationBuilder;
        private readonly IServiceCollection service;
        public DescribeNetbsadQuery()
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
        public async Task ItShouldReturnValueWhenThereIsNoException()
        {
            await Task.Run(() =>
            {
                var query = new QueryFlow<Netbsad>(mockApplicationBuilder.Object);
                var result = query.GetListAsync("", "NETBSAD").Result;

                Assert.NotNull(result);
            });
        }

    }
}
