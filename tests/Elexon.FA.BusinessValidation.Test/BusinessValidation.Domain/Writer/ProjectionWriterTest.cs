using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
using Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD;
using Elexon.FA.Core.Blob.Abstractions;
using Elexon.FA.Core.HttpClient;
using Elexon.FA.Core.KeyVault.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Writer
{
    public class ProjectionWriterTest
    {
        private Mock<IApplicationBuilder> mockApplicationBuilder;
        IServiceCollection service;

        public ProjectionWriterTest()
        {
            service = new ServiceCollection();
            service.AddSingleton<ICoreBlobStorage, MockCoreBlobStorage>();
            service.AddSingleton<ICoreKeyVaultClient, MockCoreKeyVaultClient>();
            service.AddSingleton<ICoreHttpServiceClient, MockCoreHttpServiceClient>();
            mockApplicationBuilder = new Mock<IApplicationBuilder>();
            mockApplicationBuilder.SetupGet(a => a.ApplicationServices).Returns(service.BuildServiceProvider());
        }

        [Fact]
        public async Task ProjectionWriter_UpLoadFile_Should_Return_TaskComplete_When_There_Is_No_Exception()
        {
            await Task.Run(() =>
            {
                var writer = new ProjectionWriter(mockApplicationBuilder.Object);
                var result = writer.UpLoadFile("", "");
                Assert.True(result.IsCompleted);
            });
        }

        [Fact]
        public async Task ProjectionWriter_UpdateFileStatus_Should_Return_TaskComplete_When_There_Is_No_Exception()
        {
            await Task.Run(() =>
            {
                var writer = new ProjectionWriter(mockApplicationBuilder.Object);
                StatusTableEntity statusTableEntity = new StatusTableEntity();
                var result = writer.UpdateFileStatus(statusTableEntity);
                Assert.True(result.IsCompleted);
            });
        }

        [Fact]

        public async Task Should_Return_TaskComplete_When_Inserting_Values_In_BoalfIndexPath()
        {
            await Task.Run(() =>
            {
                var writer = new ProjectionWriter(mockApplicationBuilder.Object);
                List<BoalfIndexTable> boalfIndexTable = new List<BoalfIndexTable>()
                {
                    new BoalfIndexTable
                    {
                        PartitionKey = "1",
                        RowKey = "2",
                         BmuName="TimeFromAndTimeTo",
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                    }
                };

                var result = writer.InsertBoalfIndex(boalfIndexTable);
                Assert.True(result.IsCompleted);
            });
        }

        [Fact]

        public async Task Should_Return_TaskComplete_When_UpLoadingFile_For_Boalf()
        {
            await Task.Run(() =>
            {
                var writer = new ProjectionWriter(mockApplicationBuilder.Object);
                List<Boalf> boalfs = new List<Boalf>
            {
 new Boalf
                {
                   BmuName="TimeFromAndTimeTo",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,14,00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                } };

                var result = writer.UpLoadFileBoalf(boalfs,"path");
                Assert.True(result.IsCompleted);
            });
        }

        [Fact]

        public async Task Should_Delete_Blob_On_Passing_BlobName()
        {
            await Task.Run(() =>
            {
                var writer = new ProjectionWriter(mockApplicationBuilder.Object);
                
                var result = writer.DeleteBlob("Boalf");
                Assert.True(result.IsCompleted);
            });
        }
    }
}
