using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.BusinessValidation.FPNFlow.Command;
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

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.FPN.CommandHandler
{
    public class ValidatedFpnCommandHandlerTest : IDisposable
    {
        private Mock<IQueryFlow<Fpn>> _mockQuery = new Mock<IQueryFlow<Fpn>>();
        private IServiceCollection _service;
        private Mock<IProjectionWriter> _mockWriter = new Mock<IProjectionWriter>();
        private Mock<IApplicationBuilder> _mockApplicationBuilder = new Mock<IApplicationBuilder>();
        private FpnMockData _mockData;

        public ValidatedFpnCommandHandlerTest()
        {
            _mockData = new FpnMockData();
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
            System.Collections.Generic.List<Fpn> fpns = _mockData.GetFpns();
            System.Collections.Generic.List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
            fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
            fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 2, 30, 00);
            Item item = new Item()
            {
                ItemPath = "Inbound/SAA-I003-FPN/2018/10/24/29/FPN/PN.json",
                ItemId = "FPN"
            };
            string blobName = "Processing/SAA-I003-FPN/2018/10/24/29/FPN/PN.json";
            ValidatedFpnCommand command = new ValidatedFpnCommand(item);
            _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(fpns));
            _mockQuery.Setup(s => s.ExistsAsync(blobName)).Returns(Task.FromResult(true));
            _mockQuery.Setup(s => s.GetBmuParticipationAsync(fpns.FirstOrDefault().TimeFrom, fpns.FirstOrDefault().TimeTo)).Returns(Task.FromResult(bmuUnit));
            _mockWriter.Setup(s => s.UpLoadFile(fpns, "")).Returns(Task.CompletedTask);
            ValidatedFpnCommandHandler commandHandler = new ValidatedFpnCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
            BusinessValidationProxy result = await commandHandler.Handle(command, new CancellationToken() { });

            Assert.False(result.ValidationResult);
        }

        [Fact]
        public async Task ValidatedBodCommandHandler_Handle_Should_Return_False_When_WaringCheck_IsFalse()
        {
            System.Collections.Generic.List<Fpn> fpns = _mockData.GetFpns();
            System.Collections.Generic.List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
            fpns[0].TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
            fpns[1].TimeFrom = new DateTime(2018, 11, 10, 1, 30, 00);
            Item item = new Item()
            {
                ItemPath = "Inbound/SAA-I003-FPN/2018/10/24/29/FPN/PN.json",
                ItemId = "FPN"
            };
            string blobName = "Processing/SAA-I003-FPN/2018/11/10/3/FPN/PN.json";
            ValidatedFpnCommand command = new ValidatedFpnCommand(item);
            _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(fpns));
            _mockQuery.Setup(s => s.ExistsAsync(blobName)).Returns(Task.FromResult(true));
            _mockQuery.Setup(s => s.GetBmuParticipationAsync(fpns.FirstOrDefault().TimeFrom, fpns.FirstOrDefault().TimeTo)).Returns(Task.FromResult(bmuUnit));
            _mockWriter.Setup(s => s.UpLoadFile(fpns, "")).Returns(Task.CompletedTask);
            ValidatedFpnCommandHandler commandHandler = new ValidatedFpnCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
            BusinessValidationProxy result = await commandHandler.Handle(command, new CancellationToken() { });

            Assert.False(result.ValidationResult);
        }

        [Fact]
        public async Task ValidatedBodCommandHandler_Handle_Should_Return_True_When_ErrorCheck_WarningCheck_RuleSet_Are_True()
        {
            System.Collections.Generic.List<Fpn> fpns = _mockData.GetFpns();
            System.Collections.Generic.List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
            fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
            fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 10, 00);
            Item item = new Item()
            {
                ItemPath = "Inbound/SAA-I003-FPN/2018/10/24/29/FPN/PN.json",
                ItemId = "FPN"
            };
            string blobName = "Processing/SAA-I003-FPN/2018/11/24/3/FPN/PN.json";
            ValidatedFpnCommand command = new ValidatedFpnCommand(item);
            _mockQuery.Setup(s => s.GetListAsync(command.Items.ItemPath, command.Items.ItemId)).Returns(Task.FromResult(fpns));
            _mockQuery.Setup(s => s.ExistsAsync(blobName)).Returns(Task.FromResult(true));
            _mockQuery.Setup(s => s.GetBmuParticipationAsync(fpns.FirstOrDefault().TimeFrom, fpns.FirstOrDefault().TimeTo)).Returns(Task.FromResult(bmuUnit));
            _mockWriter.Setup(s => s.UpLoadFile(fpns, "")).Returns(Task.CompletedTask);
            ValidatedFpnCommandHandler commandHandler = new ValidatedFpnCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
            BusinessValidationProxy result = await commandHandler.Handle(command, new CancellationToken() { });

            Assert.True(result.ValidationResult);
        }
    }
}
