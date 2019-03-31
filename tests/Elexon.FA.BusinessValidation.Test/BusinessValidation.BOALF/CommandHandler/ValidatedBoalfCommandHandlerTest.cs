using Elexon.FA.BusinessValidation.BOALFFlow.Command;
using Elexon.FA.BusinessValidation.BOALFlow.FileProcess;
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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF.CommandHandler
{
    public class ValidatedBoalfCommandHandlerTest
    {
        private readonly Mock<IQueryFlow<Boalf>> _mockQuery;
        private readonly IServiceCollection _service;
        private readonly Mock<IProjectionWriter> _mockWriter = new Mock<IProjectionWriter>();
        private readonly Mock<IApplicationBuilder> _mockApplicationBuilder;
        private readonly Mock<IFileProcessService> _mockFileProcessorService;
        private readonly BoalfMockData _mockData;
        public ValidatedBoalfCommandHandlerTest()
        {
            _mockQuery = new Mock<IQueryFlow<Boalf>>();
            _mockData = new BoalfMockData();
            _service = new ServiceCollection();
            _service.AddSingleton<ICoreKeyVaultClient, MockCoreKeyVaultClient>();
            _mockApplicationBuilder = new Mock<IApplicationBuilder>();
            _mockApplicationBuilder.SetupGet(a => a.ApplicationServices).Returns(_service.BuildServiceProvider());
            _mockFileProcessorService = new Mock<IFileProcessService>();
        }

        [Fact]
        public async Task ValidatedBoalfCommandHandler_Handle_Should_Return_False_When_No_Record_To_Validate()
        {

            await Task.Run(() =>
            {
                List<Boalf> boalf = new List<Boalf>();
                List<ParticipantEnergyAsset> participant = new List<ParticipantEnergyAsset>();
                Item item = new Item()
                {
                    ItemPath = "Inbound/path",
                    ItemId = "location"
                };
                ValidatedBoalfCommand command = new ValidatedBoalfCommand(item);
                _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(boalf));
                _mockQuery.Setup(s => s.GetBmuParticipationAsync(DateTime.Now, DateTime.Now)).Returns(Task.FromResult(participant));
                _mockWriter.Setup(s => s.UpLoadFile(boalf, "")).Returns(Task.CompletedTask);
                ValidatedBoalfCommandHandler commandHandler = new ValidatedBoalfCommandHandler(_mockQuery.Object, _mockApplicationBuilder.Object, _mockFileProcessorService.Object);

                BusinessValidationProxy result = commandHandler.Handle(command, new CancellationToken() { }).Result;
                Assert.False(result.ValidationResult);
            });
        }

        [Fact]
        public async Task ValidatedBoalfCommandHandler_Handle_Should_Return_False_When_WaringCheck_IsFalse()
        {
            List<Boalf> boalfs = _mockData.GetBoalfs().Take(1).ToList();
            List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
            List<BoalfIndexTable> boalfIndexTable = _mockData.GetUpdateorINSForFileProcess();
            boalfs[0].DeemedBidOfferFlag = "FALSE";
            boalfs[0].SoFlag = "FALSE";
            boalfs[0].AmendmentFlag = "FALSE";
            boalfs[0].StorFlag = "FALSE";
            boalfs[0].TimeFrom = DateTime.Now;
            boalfs[0].TimeTo = DateTime.Now;
            Item item = new Item()
            {
                ItemPath = "Test/SAA-I00V-Boalf/2018/10/24/29/Boalf/BOALF.json",
                ItemId = "BOALF"
            };
            ValidatedBoalfCommand command = new ValidatedBoalfCommand(item);
            _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(boalfs));
            _mockQuery.Setup(s => s.GetBmuParticipationAsync(boalfs.FirstOrDefault().TimeFrom, boalfs.FirstOrDefault().TimeTo)).Returns(Task.FromResult(bmuUnit));
            _mockWriter.Setup(s => s.UpLoadFile(boalfs, "")).Returns(Task.CompletedTask);
            _mockQuery.Setup(s => s.GetListBoalfIndexTable(boalfs.FirstOrDefault().BmuName, boalfs.FirstOrDefault().BidOfferAcceptanceNumber.ToString(), boalfs.FirstOrDefault().AcceptanceTime.ToString("yyyy-MM-dd HH:mm"))).Returns(Task.FromResult(boalfIndexTable));
            ValidatedBoalfCommandHandler commandHandler = new ValidatedBoalfCommandHandler(_mockQuery.Object, _mockApplicationBuilder.Object, _mockFileProcessorService.Object);
            BusinessValidationProxy result = await commandHandler.Handle(command, new CancellationToken() { });

            Assert.False(result.ValidationResult);
        }

    }
}
