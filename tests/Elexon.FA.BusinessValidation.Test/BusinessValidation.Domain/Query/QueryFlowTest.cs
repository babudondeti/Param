using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF;
using Elexon.FA.Core.Blob.Abstractions;
using Elexon.FA.Core.HttpClient;
using Elexon.FA.Core.KeyVault.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Query
{
    public class QueryFlowTest:IDisposable
    {
        private Mock<IApplicationBuilder> mockApplicationBuilder;
        private readonly IServiceCollection service;   
        private BoalfMockData _boalfMockData;

        public QueryFlowTest()
        {
            _boalfMockData = new BoalfMockData();
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
            _boalfMockData = null;
            mockApplicationBuilder = null;
        }
        [Fact]
        public async Task BoalfQuery_GetBoalfListAsync_Should_Return_Value_When_There_Is_No_Exception_For_Boalf()
        {
            var query = new QueryFlow<Boalf>(mockApplicationBuilder.Object);
            var result = await query.GetListAsync("path", "BOALF");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task BoalfQuery_GetBMUParticipationAsync_Should_Return_Value_When_There_Is_No_Exception()
        {
            var query = new QueryFlow<Boalf>(mockApplicationBuilder.Object);
            var boalf = _boalfMockData.GetBoalfs().FirstOrDefault();          
            boalf.TimeFrom = new DateTime(2018, 11, 10, 14, 00, 00);
            boalf.TimeTo = new DateTime(2018, 11, 10, 14, 30, 00);

            var result = await query.GetBmuParticipationAsync(boalf.TimeFrom, boalf.TimeTo);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Delete_When_There_Is_No_Exception()
        {
            await Task.Run(() =>
            {
                var query = new QueryFlow<Boalf>(mockApplicationBuilder.Object);
                var result = query.DeleteAsync("BOALF");

                Assert.NotNull(result);
            });
        }

        [Fact]
        public async Task Should_GetListBoalfIndexTable_When_There_Is_No_Exception()
        {
            await Task.Run(() =>
            {
                var query = new QueryFlow<Boalf>(mockApplicationBuilder.Object);
                var boalf = _boalfMockData.GetBoalfs().FirstOrDefault();
                boalf.AcceptanceTime = new DateTime(2018, 11, 10, 14, 00, 00);
                string acceptanceTime = boalf.AcceptanceTime.ToString();

                var result = query.GetListBoalfIndexTable("BMU","1",acceptanceTime);

                Assert.NotNull(result);
            });
        }

        [Fact]

        public async Task Should_Delete_Blob_On_Passing_BlobName()
        {
            await Task.Run(() =>
            {
                var query = new QueryFlow<Boalf>(mockApplicationBuilder.Object);

                var result = query.DeleteAsync("Boalf");
                Assert.True(result.IsCompleted);
            });
        }
    }
}
