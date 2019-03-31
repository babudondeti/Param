using Elexon.FA.BusinessValidation.Api.Configurations;
using Elexon.FA.BusinessValidation.BOALFlow.FileProcess;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.Core.HttpClient;
using Elexon.FA.Core.KeyVault.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Api.Configurations
{
    public class ServiceCollectionExtensionTest
    {
        readonly IServiceCollection _service;
        readonly ICoreKeyVaultClient _keyVaultClient;
        private readonly Mock<IMediator> _mockMediator;
        public ServiceCollectionExtensionTest()
        {
            _service = new ServiceCollection();
            _keyVaultClient = new MockCoreKeyVaultClients();
            _mockMediator = new Mock<IMediator>();
        }
        [Fact]
        public async Task Should_ExecuteRegisterBlobwithoutException()
        {
            await Task.Run(() =>
            {
                _service.RegisterBlob(_keyVaultClient);
                _service.RegisterDependencies(_keyVaultClient);
                ServiceProvider builder = _service.BuildServiceProvider();
                var coreHttpServiceClient = builder.GetService<ICoreHttpServiceClient>();
                Assert.NotNull(coreHttpServiceClient);
                Assert.IsType<CoreHttpServiceClient>(coreHttpServiceClient);
                var applicationBuilder = builder.GetService<IApplicationBuilder>();
                Assert.NotNull(applicationBuilder);
                Assert.IsType<ApplicationBuilder>(applicationBuilder);
                var coreKeyVaultClient = builder.GetService<ICoreKeyVaultClient>();
                Assert.NotNull(coreKeyVaultClient);          
                var queryFlowBod = builder.GetService<IQueryFlow<Bod>>();
                Assert.NotNull(queryFlowBod);
                Assert.IsType<QueryFlow<Bod>>(queryFlowBod);
                var queryFlowBoalf = builder.GetService<IQueryFlow<Boalf>>();
                Assert.NotNull(queryFlowBoalf);
                Assert.IsType<QueryFlow<Boalf>>(queryFlowBoalf);
                var queryFlowNetbsad = builder.GetService<IQueryFlow<Netbsad>>();
                Assert.NotNull(queryFlowNetbsad);
                Assert.IsType<QueryFlow<Netbsad>>(queryFlowNetbsad);
                var queryFlowDisbsad = builder.GetService<IQueryFlow<Disbsad>>();
                Assert.NotNull(queryFlowDisbsad);
                Assert.IsType<QueryFlow<Disbsad>>(queryFlowDisbsad);
                var queryFlowFpn = builder.GetService<IQueryFlow<Fpn>>();
                Assert.NotNull(queryFlowFpn);
                Assert.IsType<QueryFlow<Fpn>>(queryFlowFpn);
                var projectionWriter = builder.GetService<IProjectionWriter>();
                Assert.NotNull(projectionWriter);
                Assert.IsType<ProjectionWriter>(projectionWriter);               
                var fileProcessService = builder.GetService<IFileProcessService>();
                Assert.NotNull(fileProcessService);
                Assert.IsType<FileProcessService>(fileProcessService);

            });
        }
    }
}
