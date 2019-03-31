using Elexon.FA.BusinessValidation.Api.MessageHandlers;
using Elexon.FA.BusinessValidation.Api.Strategy;
using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF;
using Elexon.FA.Core.IntegrationMessage;
using Elexon.FA.Core.KeyVault.Abstractions;
using Elexon.FA.Core.MessageHeader.Abstractions;
using Elexon.FA.Core.ServiceBus.Abstractions;
using Elexon.FA.Core.ServiceBus.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test
{
    public class MessageHandlerTest : IDisposable
    {
        private Mock<IApplicationBuilder> mockApplicationBuilder;
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IReceiveManager> receiveManagerMock;
        private readonly Mock<ICoreMessageHeader> _coreMessageHeader;
        private readonly Mock<ICoreKeyVaultClient> _coreKeyVaultClient;
        private readonly Mock<ISendManager> _sendManager;
        private readonly Mock<IBusinessValidationStrategy> _businessValidationStrategy;
        private readonly IServiceCollection service;
        private readonly BoalfMockData _mockData;

        public MessageHandlerTest()
        {
            service = new ServiceCollection();
            mockApplicationBuilder = new Mock<IApplicationBuilder>();
            _mockData = new BoalfMockData();

            mediatorMock = new Mock<IMediator>();
            receiveManagerMock = new Mock<IReceiveManager>();

            _coreMessageHeader = new Mock<ICoreMessageHeader>();
            _coreKeyVaultClient = new Mock<ICoreKeyVaultClient>();
            _sendManager = new Mock<ISendManager>();
            _businessValidationStrategy = new Mock<IBusinessValidationStrategy>();

            service.AddSingleton(mediatorMock.Object);
            service.AddSingleton<ICoreMessageHeader, MockCoreMessageHeader>();
            service.AddSingleton<ICoreKeyVaultClient, MockCoreKeyVaultClient>();
            service.AddSingleton<ISendManager, MockSendManager>();
            service.AddScoped<IBusinessValidationStrategy, BoalfStrategy>();

            service.AddSingleton<IReceiveManager, MockReceiveManager>();

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
        public async Task Should_RegisterSubscription_On_Passing_ParameterValues()
        {
            BusinessValidationProxy businessValidationProxy = await _mockData.BusinessValidationProxies();

            mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<BusinessValidationProxy>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(businessValidationProxy));
            receiveManagerMock.Setup(r => r.Receive<MessageProcessResponse>(default, default)).Returns(Task.FromResult(MessageProcessResponse.Complete));
            mockApplicationBuilder.SetupGet(a => a.ApplicationServices).Returns(service.BuildServiceProvider());
            MessageHandler msgHandler = new MessageHandler(receiveManagerMock.Object, mediatorMock.Object);
            msgHandler.RegisterSubscription();
            CoreMessage message = new CoreMessage();
            string str = "{\"ItineraryData\":{\"Items\":null},\"ServiceOutput\":{\"Items\":[{\"ItemId\":\"BOALF\",\"ItemType\":\"ExternalInterface\",\"ItemLocation\":\"External\",\"ItemPath\":\"Test/SAA-I00V-Boalf/2018/10/24/29/Boalf/BOALF.json\",\"SourceLabel\":\"genericvalidationcompleted\",\"SourceReference\":\"2018-09-21:9-BusinessValidation\",\"InlineData\":null}],\"ErrorMessage\":null}}";
            byte[] byteMessage = Encoding.UTF8.GetBytes(str);
            message.Body = byteMessage;
            message.UserProperties["PipelineName"] = "BusinessValidation";
            message.UserProperties["CurrentChain"] = "bodfailure->businessvalidationsuccesssub";
            message.UserProperties["EventScope"] = "";
            message.UserProperties["EventPeriod"] = "";
            message.UserProperties["TechnicalName"] = "BusinessValidation";
            message.UserProperties["OriginalChain"] = "bodfailure->businessvalidationsuccesssub";
            message.UserProperties["Tracing"] = "BusinessValidation~9ddb7123-5f82-4abf-905f-93a09790eaa5~201809061243|BusinessValidation~b5815556-08f9-4831-8ef7-203afa2d11fc~19/12/2018 07:51:51";


            await Task.Run(() =>
           {
               Task<MessageProcessResponse> messageResponse = msgHandler.OnProcessAsync(message);
               Assert.Equal(0, Convert.ToInt32(MessageProcessResponse.Complete));
           });
        }

        [Fact]
        public void SendServiceOutPutMessageCommandTestWithMediatR()
        {
            string[] inlineInput = { "InlineData" };
            List<Item> item = new List<Item>()
            {
                new Item
                {
                ItemId ="BUSINESS VALIDATION Pipeline",
                InlineData=inlineInput
                }
            };
            CoreMessage message = new CoreMessage();
            string str = "test message";
            byte[] byteMessage = Encoding.UTF8.GetBytes(str);
            bool status = true;
            List<string> errorMessages = new List<string> { "Sending error message" };
            message.Body = byteMessage;
            message.UserProperties["PipelineName"] = "BusinessValidation";
            message.UserProperties["CurrentChain"] = "bodfailure->businessvalidationsuccesssub";
            message.UserProperties["EventScope"] = "";
            message.UserProperties["EventPeriod"] = "";
            message.UserProperties["TechnicalName"] = "BusinessValidation";
            message.UserProperties["OriginalChain"] = "bodfailure->businessvalidationsuccesssub";
            message.UserProperties["Tracing"] = "BusinessValidation~9ddb7123-5f82-4abf-905f-93a09790eaa5~201809061243|BusinessValidation~b5815556-08f9-4831-8ef7-203afa2d11fc~19/12/2018 07:51:51";

            Mock<IMediator> mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), new System.Threading.CancellationToken()));
            SendServiceOutPutMessageCommand expectedOutput = new SendServiceOutPutMessageCommand(item, message, status, errorMessages);
            Assert.NotNull(expectedOutput);
        }
        [Fact]
        public async Task SendServiceOutPutMessageCommandHandlerTestHandle()
        {
            bool boolenStatus = false;
            string[] inlineInput = { "InlineData" };
            List<Item> items = new List<Item>()
            {
                new Item
                {
                ItemId ="BUSINESS VALIDATION Pipeline",
                InlineData=inlineInput
                }
            };
            Mock<ICoreMessageHeader> mockCoreMessageHeader = new Mock<ICoreMessageHeader>();
            Mock<IApplicationBuilder> MockApplicationBuilder = new Mock<IApplicationBuilder>();
            MockApplicationBuilder.SetupGet(a => a.ApplicationServices).Returns(service.BuildServiceProvider());
            mockCoreMessageHeader.Setup(x => x.SendServiceOutputToTopicAsync(It.IsAny<List<Item>>(), It.IsAny<CoreMessage>())).ReturnsAsync(boolenStatus);
            SendServiceOutPutMessageCommandHandler objSendServiceOutPutMessageCommandHandler = new SendServiceOutPutMessageCommandHandler(MockApplicationBuilder.Object);
            bool status = true;
            CoreMessage message = new CoreMessage();
            string str = @"{""testKey"": ""testValue""}";
            byte[] byteMessage = Encoding.UTF8.GetBytes(str);
            List<string> errorMessages = new List<string> { "Sending error message" };
            message.Body = byteMessage;
            message.UserProperties["PipelineName"] = "BusinessValidationPipeline";
            message.UserProperties["CurrentChain"] = "bodfailure->businessvalidationsuccesssub";
            message.UserProperties["EventScope"] = "";
            message.UserProperties["EventPeriod"] = "";
            message.UserProperties["TechnicalName"] = "BusinessValidation";
            message.UserProperties["OriginalChain"] = "bodfailure->businessvalidationsuccesssub";
            message.UserProperties["Tracing"] = "BusinessValidation~9ddb7123-5f82-4abf-905f-93a09790eaa5~201809061243|BusinessValidation~b5815556-08f9-4831-8ef7-203afa2d11fc~19/12/2018 07:51:51";

            SendServiceOutPutMessageCommand ObjSendServiceOutPutMessageCommandInput = new SendServiceOutPutMessageCommand(items, message, status, errorMessages);
            MessageProcessResponse result = await objSendServiceOutPutMessageCommandHandler.Handle(ObjSendServiceOutPutMessageCommandInput, new CancellationToken());
            Assert.Equal(MessageProcessResponse.Complete, result);
        }



    }
}
