using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Domain.ProjectionWriter;
using Elexon.FA.BusinessValidation.Domain.Query;
using Elexon.FA.BusinessValidation.NETBSADFlow.Command;
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

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.NETBSADFLOW.CommandHandler
{
    public class DescribeValidatedNetbsadCommandHandler : IDisposable
    {
        private Mock<IQueryFlow<Netbsad>> _mockQuery = new Mock<IQueryFlow<Netbsad>>();
        private readonly IServiceCollection _service;
        private Mock<IProjectionWriter> _mockWriter = new Mock<IProjectionWriter>();
        private Mock<IApplicationBuilder> _mockApplicationBuilder;
        private NetbsadMockData _mockData;
        public DescribeValidatedNetbsadCommandHandler()
        {
            _mockData = new NetbsadMockData();
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
                List<Netbsad> netbsads = new List<Netbsad>();
                Item item = new Item()
                {
                    ItemPath = "Inbound/path",
                    ItemLocation = "location"
                };
                ValidatedNetbsadCommand command = new ValidatedNetbsadCommand(item);
                _mockQuery.Setup(s => s.GetListAsync(command.Item.ItemPath, command.Item.ItemLocation)).Returns(Task.FromResult(netbsads));
                _mockWriter.Setup(s => s.UpLoadFile(netbsads, "")).Returns(Task.CompletedTask);
                ValidatedNetbsadCommandHandler commandHandler = new ValidatedNetbsadCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                BusinessValidationProxy result = commandHandler.Handle(command, new CancellationToken() { }).Result;
                Assert.False(result.InValid);
            });

        }


        [Fact]
        public async Task ItShouldReturnFalseWhenAnyWaringCheckFails()
        {
            await Task.Run(() =>
            {
                List<Netbsad> netbsads = _mockData.GetNetbsads();

                Item item = new Item()
                {
                    ItemPath = "Inbound_Netbsad/SAA-I00G-Netbsad/2018/11/26/Netbsad.json",
                    ItemLocation = "location"
                };

                ValidatedNetbsadCommand command = new ValidatedNetbsadCommand(item);
                _mockQuery.Setup(s => s.GetListAsync(command.Item.ItemPath, command.Item.ItemLocation)).Returns(Task.FromResult(netbsads));
                _mockWriter.Setup(s => s.UpLoadFile(netbsads, "")).Returns(Task.CompletedTask);
                ValidatedNetbsadCommandHandler commandHandler = new ValidatedNetbsadCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                BusinessValidationProxy result = commandHandler.Handle(command, new CancellationToken() { }).Result;

                Assert.False(result.InValid);
            });

        }

        [Fact]
        public async Task ItShouldReturnTrueWhenNoWaringCheckFails()
        {
            await Task.Run(() =>
            {
                List<Netbsad> netbsads = _mockData.GetNetbsads();
                netbsads[0].SettlementPeriod = 30;

                Item item = new Item()
                {
                    ItemPath = "Inbound_Netbsad / SAA - I00G - Netbsad / 2018 / 11 / 26 / Netbsad.json",
                    ItemLocation = "location",
                    ItemId = "NETBSAD"
                };

                ValidatedNetbsadCommand command = new ValidatedNetbsadCommand(item);
                _mockQuery.Setup(s => s.GetListAsync(command.Item.ItemPath, command.Item.ItemId)).Returns(Task.FromResult(netbsads));
                _mockWriter.Setup(s => s.UpLoadFile(netbsads, "")).Returns(Task.CompletedTask);
                ValidatedNetbsadCommandHandler commandHandler = new ValidatedNetbsadCommandHandler(_mockQuery.Object, _mockWriter.Object, _mockApplicationBuilder.Object);
                BusinessValidationProxy result = commandHandler.Handle(command, new CancellationToken() { }).Result;

                Assert.True(result.Valid);
            });

        }

    }
}
