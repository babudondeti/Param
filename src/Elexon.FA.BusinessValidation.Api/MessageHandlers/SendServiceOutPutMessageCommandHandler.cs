using Elexon.FA.BusinessValidation.Domain.Seed;
using Elexon.FA.Core.IntegrationMessage;
using Elexon.FA.Core.IntegrationMessage.Events;
using Elexon.FA.Core.KeyVault.Abstractions;
using Elexon.FA.Core.Logging;
using Elexon.FA.Core.MessageHeader.SeedWork;
using Elexon.FA.Core.ServiceBus.Abstractions;
using Elexon.FA.Core.ServiceBus.Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Api.MessageHandlers
{
    /// <summary>
    /// Defines the <see cref="SendServiceOutPutMessageCommandHandler" />
    /// </summary>
    public class SendServiceOutPutMessageCommandHandler : IRequestHandler<SendServiceOutPutMessageCommand, MessageProcessResponse>
    {
        /// <summary>
        /// Defines the _sendMngr
        /// </summary>
        private readonly ISendManager sendMngr;

        /// <summary>
        /// Defines the _app
        /// </summary>
        private readonly IApplicationBuilder app;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendServiceOutPutMessageCommandHandler"/> class.
        /// </summary>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public SendServiceOutPutMessageCommandHandler(IApplicationBuilder applicationBuilder)
        {
            app = applicationBuilder;
            sendMngr = app.ApplicationServices.GetRequiredService<ISendManager>();
        }

        /// <summary>
        /// The Handle
        /// </summary>
        /// <param name="request">The request<see cref="SendServiceOutPutMessageCommand"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{MessageProcessResponse}"/></returns>
        public async Task<MessageProcessResponse> Handle(SendServiceOutPutMessageCommand request, CancellationToken cancellationToken)
        {
            ICoreKeyVaultClient keyVault = app.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
            IntegrationEvent integrationEvent = null;
            MessageProcessResponse messageProcessResponse = MessageProcessResponse.Abandon;
            try
            {
                request = request ?? throw new ArgumentNullException(nameof(request));
                integrationEvent = PrepareOutputMessage(request.Items, request.Message);
                if (request.Status)
                {
                    await sendMngr.SendAsync(await keyVault.GetSecret(BusinessValidationConstants.KEYVAULT_BUSINESSVALIDATIONSUCCESSTOPIC), integrationEvent);
                    messageProcessResponse = MessageProcessResponse.Complete;
                }
                else
                {
                    integrationEvent.Body.ServiceOutput = new ServiceOutput();
                    await sendMngr.SendAsync(await keyVault.GetSecret(BusinessValidationConstants.ErrorTopicNameKeyName), integrationEvent);
                    messageProcessResponse = MessageProcessResponse.Complete;
                }
                return messageProcessResponse;
            }
            catch (Exception ex)
            {
                integrationEvent = GetIntegrationEvent(request?.Message);
                integrationEvent.Body.ServiceOutput = new ServiceOutput();
                await sendMngr.SendAsync(await keyVault.GetSecret(BusinessValidationConstants.KEYVAULT_ERRORTOPICNAME), integrationEvent);
                Log.Error(ex.Message);
                return MessageProcessResponse.Abandon;
            }
        }

        /// <summary>
        /// The PrepareOutputMessage
        /// </summary>
        /// <param name="items">The items<see cref="List{Item}"/></param>
        /// <param name="coreMessage">The coreMessage<see cref="CoreMessage"/></param>
        /// <returns>The <see cref="IntegrationEvent"/></returns>
        private IntegrationEvent PrepareOutputMessage(IEnumerable<Item> items, CoreMessage coreMessage)
        {
            Dictionary<string, object> properties = GetUserProperties(coreMessage);
            IntegrationEvent integrationEvent = GetIntegrationEvent(coreMessage);


            ServiceOutput serviceOutput = new ServiceOutput();

            IEnumerable<Item> result = items.Select(s => new Item
            {
                ItemId = s.ItemId,
                InlineData = s.InlineData,
                SourceLabel = s.SourceLabel,
                ItemPath = s.ItemPath,
                SourceReference = SourceReference(properties),
                ItemLocation = BusinessValidationConstants.EXTERNAL,
                ItemType = BusinessValidationConstants.EXTERNALINTERFACE
            });

            serviceOutput.Items = result.ToList();
            integrationEvent.Body.ServiceOutput = serviceOutput;
            return integrationEvent;
        }

        /// <summary>
        /// The SourceReference
        /// </summary>
        /// <param name="userProperties">The userProperties<see cref="Dictionary{TKey,TValue}"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string SourceReference(IDictionary<string, object> userProperties)
        {

            return userProperties[MessageHeaderConstants.EventPeriod] + "-" + userProperties[MessageHeaderConstants.TechnicalName];
        }

        /// <summary>
        /// The GetUserProperties
        /// </summary>
        /// <param name="message">The message<see cref="CoreMessage"/></param>
        /// <returns>The <see cref="Dictionary{TKey,TValue}"/></returns>
        private static Dictionary<string, object> GetUserProperties(CoreMessage message)
        {

            Dictionary<string, object> userProperties = new Dictionary<string, object>
            {
                { nameof(MessageHeaderConstants.PipelineName), message?.UserProperties[MessageHeaderConstants.PipelineName].ToString() },
                { nameof(MessageHeaderConstants.CurrentChain), message?.UserProperties[MessageHeaderConstants.CurrentChain].ToString() },
                { nameof(MessageHeaderConstants.EventScope), message?.UserProperties[MessageHeaderConstants.EventScope].ToString() },
                { nameof(MessageHeaderConstants.EventPeriod), message?.UserProperties[MessageHeaderConstants.EventPeriod].ToString()},
                { nameof(MessageHeaderConstants.TechnicalName), message?.UserProperties[MessageHeaderConstants.TechnicalName].ToString()},
                { nameof(MessageHeaderConstants.OriginalChain), message?.UserProperties[MessageHeaderConstants.OriginalChain].ToString()}
            };

            return userProperties;
        }

        /// <summary>
        /// The GetIntegrationEvent
        /// </summary>
        /// <param name="message">The message<see cref="CoreMessage"/></param>
        /// <returns>The <see cref="IntegrationEvent"/></returns>
        private IntegrationEvent GetIntegrationEvent(CoreMessage message)
        {
            string jsonstring = Encoding.UTF8.GetString(message.Body);

            DcpMessage dcpMessage = JsonConvert.DeserializeObject<DcpMessage>(jsonstring);

            IntegrationEvent integrationEvnt = SetIntegrationEventProperties(message);
            integrationEvnt.Body = dcpMessage;

            return integrationEvnt;
        }

        /// <summary>
        /// The SetIntegrationEventProperties
        /// </summary>
        /// <param name="message">The message<see cref="CoreMessage"/></param>
        /// <returns>The <see cref="IntegrationEvent"/></returns>
        private IntegrationEvent SetIntegrationEventProperties(CoreMessage message)
        {

            IntegrationEvent integrationEvnt = new IntegrationEvent
            {
                CurrentChain = message?.UserProperties[MessageHeaderConstants.CurrentChain].ToString(),
                PipelineName = message?.UserProperties[MessageHeaderConstants.PipelineName].ToString(),
                EventScope = message?.UserProperties[MessageHeaderConstants.EventScope].ToString(),
                EventPeriod = message?.UserProperties[MessageHeaderConstants.EventPeriod].ToString(),
                TechnicalName = message?.UserProperties[MessageHeaderConstants.TechnicalName].ToString(),
                OriginalChain = message?.UserProperties[MessageHeaderConstants.OriginalChain].ToString(),
                Tracing = message?.UserProperties[MessageHeaderConstants.Tracing].ToString()

            };
            return integrationEvnt;
        }

    }
}
