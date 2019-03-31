using Elexon.FA.BusinessValidation.BODFlow.Command;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.Core.IntegrationMessage;
using Elexon.FA.Core.KeyVault.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD.CommandHandler
{
    public class ValidatedBodCommandHandlerTest : IDisposable
    {
        private Mock<IQueryFlow<Bod>> _mockQuery = new Mock<IQueryFlow<Bod>>();
        private readonly IServiceCollection _service;
        private Mock<IProjectionWriter> _mockWriter = new Mock<IProjectionWriter>();
        private Mock<IApplicationBuilder> _mockApplicationBuilder;
        private BodMockData _mockData;
        public ValidatedBodCommandHandlerTest()
        {
            _mockData = new BodMockData();
            _service = new ServiceCollection();
            _service.AddSingleton<ICoreKeyVaultClient, MockCoreKeyVaultClient>();
            _mockApplicationBuilder = new Mock<IApplicationBuilder>();
            _mockApplicationBuilder.SetupGet(a => a.ApplicationServices).Returns(_service.BuildServiceProvider());
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
        }

        [Fact]
        public async Task ValidatedBodCommandHandler_Handle_Should_Return_False_When_ErrorCheck_RuleSet_IsFail()
        {
            System.Collections.Generic.List<Bod> bods = _mockData.GetBods();
            System.Collections.Generic.List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
            bods.FirstOrDefault().TimeFrom = new DateTime(2018, 12, 10, 0, 00, 00);
            bods.FirstOrDefault().TimeTo = new DateTime(2018, 12, 10, 0, 30, 00);
            Item item = new Item()
            {
                ItemPath = "Inbound/SAA-I003-BOD/2018/10/24/29/BOD/BOD_Data.json",
                ItemId = "BOD"
            };
            ValidatedBodCommand command = new ValidatedBodCommand(item);
            _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(bods));
            _mockQuery.Setup(s => s.GetBmuParticipationAsync(bods.FirstOrDefault().TimeFrom, bods.FirstOrDefault().TimeTo)).Returns(Task.FromResult(bmuUnit));
            _mockWriter.Setup(s => s.UpLoadFile(bods, "")).Returns(Task.CompletedTask);
            ValidatedBodCommandHandler commandHandler = new ValidatedBodCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
            BusinessValidationProxy result = await commandHandler.Handle(command, new CancellationToken() { });

            Assert.False(result.ValidationResult);
        }

        [Fact]
        public async Task ValidatedBodCommandHandler_Handle_Should_Return_False_When_WaringCheck_IsFalse()
        {
            System.Collections.Generic.List<Bod> bods = _mockData.GetBods();
            System.Collections.Generic.List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
            bods[0].PairId = -6;
            bods[1].PairId = 0;
            bods[6].PairId = 6;
            Item item = new Item()
            {
                ItemPath = "Inbound/SAA-I003-BOD/2018/10/24/29/BOD/BOD_Data.json",
                ItemId = "BOD"
            };
            ValidatedBodCommand command = new ValidatedBodCommand(item);
            _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(bods));
            _mockQuery.Setup(s => s.GetBmuParticipationAsync(bods.FirstOrDefault().TimeFrom, bods.FirstOrDefault().TimeTo)).Returns(Task.FromResult(bmuUnit));
            _mockWriter.Setup(s => s.UpLoadFile(bods, "")).Returns(Task.CompletedTask);
            ValidatedBodCommandHandler commandHandler = new ValidatedBodCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
            BusinessValidationProxy result = await commandHandler.Handle(command, new CancellationToken() { });

            Assert.False(result.ValidationResult);
        }

        [Fact]
        public async Task ValidatedBodCommandHandler_Handle_Should_Return_True_When_ErrorCheck_WarningCheck_RuleSet_Are_True()
        {
            System.Collections.Generic.List<Bod> bods = _mockData.GetBods();
            System.Collections.Generic.List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
            bods[0].PairId = -5;
            bods[1].PairId = -4;
            bods[6].PairId = 2;
            Item item = new Item()
            {
                ItemPath = "Inbound/SAA-I003-BOD/2018/10/24/29/BOD/BOD_Data.json",
                ItemId = "BOD"

            };
            ValidatedBodCommand command = new ValidatedBodCommand(item);
            _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(bods));
            _mockQuery.Setup(s => s.GetBmuParticipationAsync(bods.FirstOrDefault().TimeFrom, bods.FirstOrDefault().TimeTo)).Returns(Task.FromResult(bmuUnit));
            _mockWriter.Setup(s => s.UpLoadFile(bods, "")).Returns(Task.CompletedTask);
            ValidatedBodCommandHandler commandHandler = new ValidatedBodCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
            BusinessValidationProxy result = await commandHandler.Handle(command, new CancellationToken() { });

            Assert.True(result.ValidationResult);
        }
    }
}
