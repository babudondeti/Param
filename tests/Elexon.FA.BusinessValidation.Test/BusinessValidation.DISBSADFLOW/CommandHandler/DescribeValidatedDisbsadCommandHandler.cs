using Elexon.FA.BusinessValidation.DISBSADFlow.Command;
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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.DISBSADFLOW.CommandHandler
{
    public class DescribeValidatedDisbsadCommandHandler : IDisposable
    {
        private Mock<IQueryFlow<Disbsad>> _mockQuery = new Mock<IQueryFlow<Disbsad>>();
        private readonly IServiceCollection _service;
        private Mock<IProjectionWriter> _mockWriter = new Mock<IProjectionWriter>();
        private Mock<IApplicationBuilder> _mockApplicationBuilder;
        private DisbsadMockData _mockData;
        public DescribeValidatedDisbsadCommandHandler()
        {
            _mockData = new DisbsadMockData();
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
        public async Task ItShouldReturnFalseWhenNoRecordFound()
        {
            await Task.Run(() =>
            {
                List<Disbsad> disbsads = new List<Disbsad>();
                Item item = new Item()
                {
                    ItemPath = "Inbound/path",
                    ItemLocation = "location"
                };
                ValidatedDisbsadCommand command = new ValidatedDisbsadCommand(item);
                _mockQuery.Setup(s => s.GetListAsync(command.Item.ItemPath, command.Item.ItemLocation)).Returns(Task.FromResult(disbsads));
                _mockWriter.Setup(s => s.UpLoadFile(disbsads, "")).Returns(Task.CompletedTask);
                ValidatedDisbsadCommandHandler commandHandler = new ValidatedDisbsadCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                BusinessValidationProxy result = commandHandler.Handle(command, new CancellationToken() { }).Result;
                Assert.False(result.InValid);
            });

        }



        [Fact]
        public async Task ItShouldReturnFalseWhenAnyWaringCheckFails()
        {
            await Task.Run(() =>
            {
                List<Disbsad> disbsads = _mockData.GetDibsads();

                Item item = new Item()
                {
                    ItemPath = "Inbound/path",
                    ItemLocation = "location"
                };

                ValidatedDisbsadCommand command = new ValidatedDisbsadCommand(item);
                _mockQuery.Setup(s => s.GetListAsync(command.Item.ItemPath, command.Item.ItemLocation)).Returns(Task.FromResult(disbsads));
                _mockWriter.Setup(s => s.UpLoadFile(disbsads, "")).Returns(Task.CompletedTask);
                ValidatedDisbsadCommandHandler commandHandler = new ValidatedDisbsadCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                BusinessValidationProxy result = commandHandler.Handle(command, new CancellationToken() { }).Result;

                Assert.False(result.InValid);
            });

        }

        [Fact]
        public async Task ItShouldReturnTrueWhenNoWaringCheckFails()
        {
            await Task.Run(() =>
            {
                List<Disbsad> disbsads = _mockData.GetDibsads();
                disbsads[0].SettlementPeriod = 30;
                disbsads[1].StorFlag = "true";

                Item item = new Item()
                {
                    ItemPath = "Inbound_Disbsad/SAA-I00G-Disbsad/2018/11/26/Disbsad.json",
                    ItemLocation = "location",
                    ItemId = "DISBSAD"
                };

                ValidatedDisbsadCommand command = new ValidatedDisbsadCommand(item);
                _mockQuery.Setup(s => s.GetListAsync(command.Item.ItemPath, command.Item.ItemId)).Returns(Task.FromResult(disbsads));
                _mockWriter.Setup(s => s.UpLoadFile(disbsads, "")).Returns(Task.CompletedTask);
                ValidatedDisbsadCommandHandler commandHandler = new ValidatedDisbsadCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                BusinessValidationProxy result = commandHandler.Handle(command, new CancellationToken() { }).Result;

                Assert.True(result.Valid);
            });

        }

    }
}
