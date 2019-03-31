using Elexon.FA.BusinessValidation.BOALFlow.FileProcess;
using Elexon.FA.BusinessValidation.Domain.Aggregate;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.Core.IntegrationMessage;
using Elexon.FA.Core.KeyVault.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF.FileProcess
{
    public class FileProcessServiceTest : IDisposable
    {
        private BoalfMockData _mockData;
        private Mock<IQueryFlow<Boalf>> _mockQuery;
        private readonly IServiceCollection _service;
        private Mock<IProjectionWriter> _mockWriter;
        private Mock<IApplicationBuilder> _mockApplicationBuilder;
        private Mock<IFileProcessService> _mockFileProcessorService;

        public FileProcessServiceTest()
        {
            _service = new ServiceCollection();
            _service.AddSingleton<ICoreKeyVaultClient, MockCoreKeyVaultClient>();
            _mockApplicationBuilder = new Mock<IApplicationBuilder>();
            _mockApplicationBuilder.SetupGet(a => a.ApplicationServices).Returns(_service.BuildServiceProvider());
            _mockFileProcessorService = new Mock<IFileProcessService>();
            _mockData = new BoalfMockData();
            _mockWriter = new Mock<IProjectionWriter>();
            _mockQuery = new Mock<IQueryFlow<Boalf>>();
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
            _mockData = null;
            _mockQuery = null;
            _mockWriter = null;
            _mockApplicationBuilder = null;
            _mockFileProcessorService = null;
        }
        [Fact]
        public async Task FileProcessService_FileProcess_Should_Return_True_When_No_Record_To_Validate()
        {
            await Task.Run(() =>
            {
                List<Boalf> boalfs = new List<Boalf>();
                List<ParticipantEnergyAsset> bmuUnit = new List<ParticipantEnergyAsset>();
                List<BoalfIndexTable> updateOrInsFlow = new List<BoalfIndexTable>();
                Item item = new Item()
                {
                    ItemPath = "Test/BOALF/2018/10/24/29/BOALF.json",
                    ItemId = "BOALF"
                };
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(item, boalfs, bmuUnit, updateOrInsFlow);
                FileProcessService fileProcess = new FileProcessService(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                Task result = fileProcess.FileProcess(aggregate);
                Assert.True(result.IsCompleted);
            });
        }
        [Fact]
        public async Task FileProcessService_FileProcess_Should_Return_Complete_When_FirstTimeInsert()
        {
            await Task.Run(() =>
            {
                List<Boalf> boalfs = _mockData.GetFileProcessData().Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = new List<BoalfIndexTable>();
                List<Boalf> blobBoalfData = new List<Boalf>();
                Item item1 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/1/BOALF.json",
                    ItemId = "BOALFS"
                };
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(item1, boalfs, bmuUnit, updateOrInsFlow);
                aggregate.ValidFlow.AddRange(aggregate.BusinessValidationFlow);
                aggregate.InValidFlow.AddRange(_mockData.GetFileProcessDataRejectedAcceptance());
                aggregate.SettlementDuration = 30;
                _mockWriter.Setup(s => s.UpLoadFile(aggregate, "")).Returns(Task.CompletedTask);
                _mockQuery.Setup(s => s.GetListAsync(item1.ItemPath, "BOALFS")).Returns(Task.FromResult(blobBoalfData));
                FileProcessService fileProcess = new FileProcessService(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                Task result = fileProcess.FileProcess(aggregate);
                Assert.True(result.IsCompleted);
            });
        }

        [Fact]
        public async Task FileProcessService_FileProcess_Should_Return_Complete_When_FileProcessCompleteWithUpdateoneAcceptance()
        {
            await Task.Run(() =>
            {
                List<Boalf> boalfs = _mockData.GetFileProcessData().Skip(2).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSForFileProcess().Skip(2).ToList();
                List<Boalf> blobBoalfData = _mockData.GetBlobDataForFileProcess().Skip(2).ToList();
                Item item1 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/1/BOALF.json",
                    ItemId = "BOALF"
                };
                Item item2 = new Item()
                {
                    ItemPath = "Processing/BOALF/2018/4/9/2/BOALF.json",
                    ItemId = "BOALFS"
                };
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(item1, boalfs, bmuUnit, updateOrInsFlow);
                aggregate.ValidFlow.AddRange(aggregate.BusinessValidationFlow);
                aggregate.InValidFlow.AddRange(_mockData.GetFileProcessDataRejectedAcceptance());
                aggregate.SettlementDuration = 30;
                _mockWriter.Setup(s => s.UpLoadFile(aggregate, "")).Returns(Task.CompletedTask);
                _mockQuery.Setup(s => s.GetListAsync(item1.ItemPath, "BOALFS")).Returns(Task.FromResult(blobBoalfData));
                _mockQuery.Setup(s => s.GetListAsync(item2.ItemPath, "BOALFS")).Returns(Task.FromResult(blobBoalfData));
                FileProcessService fileProcess = new FileProcessService(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                Task result = fileProcess.FileProcess(aggregate);
                Assert.True(result.IsCompleted);
            });
        }
        [Fact]
        public async Task FileProcessService_FileProcess_Should_Return_Complete_When_InsertMultipleAcceptancesWithoutUpdate()
        {
            await Task.Run(() =>
            {
                List<Boalf> boalfs = _mockData.GetFileProcessData().Skip(4).Take(3).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = new List<BoalfIndexTable>();
                List<Boalf> blobBoalfData = new List<Boalf>();
                Item item1 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/1/BOALF.json",
                    ItemId = "BOALFS"
                };
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(item1, boalfs, bmuUnit, updateOrInsFlow);
                aggregate.ValidFlow.AddRange(aggregate.BusinessValidationFlow);
                aggregate.InValidFlow.AddRange(_mockData.GetFileProcessDataRejectedAcceptance());
                aggregate.SettlementDuration = 30;
                _mockWriter.Setup(s => s.UpLoadFile(aggregate, "")).Returns(Task.CompletedTask);
                _mockQuery.Setup(s => s.GetListAsync(item1.ItemPath, "BOALFS")).Returns(Task.FromResult(blobBoalfData));
                FileProcessService fileProcess = new FileProcessService(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                Task result = fileProcess.FileProcess(aggregate);
                Assert.True(result.IsCompleted);
            });
        }
        [Fact]
        public async Task FileProcessService_FileProcess_Should_Return_Complete_WhenUpdateReceivedWithOneAcceptance()
        {
            await Task.Run(() =>
            {
                List<Boalf> boalfs = _mockData.GetFileProcessData().Skip(7).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSForFileProcess().Skip(4).Take(3).ToList();
                List<Boalf> blobBoalfData = _mockData.GetBlobDataForFileProcess().Skip(4).Take(3).ToList();
                Item item1 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/1/BOALF.json",
                    ItemId = "BOALFS"
                };
                Item item2 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/2/BOALF.json",
                    ItemId = "BOALFS"
                };
                Item item3 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/3/BOALF.json",
                    ItemId = "BOALFS"
                };
                Item item4 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/4/BOALF.json",
                    ItemId = "BOALFS"
                };
                Item item5 = new Item
                {
                    ItemPath = "Processing/BOALF/2018/4/9/5/BOALF.json",
                    ItemId = "BOALFS"
                };
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(item1, boalfs, bmuUnit, updateOrInsFlow);
                aggregate.ValidFlow.AddRange(aggregate.BusinessValidationFlow);
                aggregate.InValidFlow.AddRange(_mockData.GetFileProcessDataRejectedAcceptance());
                aggregate.SettlementDuration = 30;
                _mockWriter.Setup(s => s.UpLoadFile(aggregate, "")).Returns(Task.CompletedTask);
                _mockQuery.Setup(s => s.GetListAsync(item1.ItemPath, "BOALF")).Returns(Task.FromResult(blobBoalfData));
                _mockQuery.Setup(s => s.GetListAsync(item2.ItemPath, "BOALF")).Returns(Task.FromResult(blobBoalfData));
                _mockQuery.Setup(s => s.GetListAsync(item3.ItemPath, "BOALF")).Returns(Task.FromResult(blobBoalfData));
                _mockQuery.Setup(s => s.GetListAsync(item4.ItemPath, "BOALF")).Returns(Task.FromResult(blobBoalfData));
                _mockQuery.Setup(s => s.GetListAsync(item5.ItemPath, "BOALF")).Returns(Task.FromResult(blobBoalfData));
                FileProcessService fileProcess = new FileProcessService(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                Task result = fileProcess.FileProcess(aggregate);
                Assert.True(result.IsCompleted);
            });
        }
    }
}
