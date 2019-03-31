namespace Elexon.FA.BusinessValidation.Api.MessageHandlers
{
    using Api.Strategy;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.Exceptions;
    using Elexon.FA.Core.IntegrationMessage;
    using Elexon.FA.Core.Logging;
    using Elexon.FA.Core.ServiceBus.Abstractions;
    using Elexon.FA.Core.ServiceBus.Common;
    using MediatR;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="MessageHandler" />
    /// </summary>
    public class MessageHandler
    {
        /// <summary>
        /// Defines the mediator
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Defines the receiveManager
        /// </summary>
        private readonly IReceiveManager receiveManager;

        /// <summary>
        /// Defines the getStrategy
        /// </summary>
        private readonly GetStrategy getStrategy = new GetStrategy();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHandler"/> class.
        /// </summary>
        /// <param name="receiveManager">The receiveManager<see cref="IReceiveManager"/></param>
        /// <param name="mediator">The mediator<see cref="IMediator"/></param>
        public MessageHandler(IReceiveManager receiveManager, IMediator mediator)
        {
            this.receiveManager = receiveManager;
            this.mediator = mediator;
        }

        /// <summary>
        /// The RegisterSubscription
        /// </summary>
        public void RegisterSubscription()
        {
            receiveManager.Receive<CoreMessage>(OnProcessAsync, OnError);
        }

        /// <summary>
        /// The OnError
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        private static void OnError(Exception ex)
        {
            Log.Error(ex.Message);
        }

        /// <summary>
        /// The OnProcessAsync
        /// </summary>
        /// <param name="message">The message<see cref="CoreMessage"/></param>
        /// <returns>The <see cref="MessageProcessResponse"/></returns>
        public async Task<MessageProcessResponse> OnProcessAsync(CoreMessage message)
        {
            try
            {
                Guard.AgainstNull(nameof(message.Body), message?.Body);
                string msgBody = Encoding.UTF8.GetString(message?.Body);
                MessageProcessResponse messageProcessResponse = MessageProcessResponse.Abandon;
                Log.DependencyTrackingTelemetryModule();
                DcpMessage dcpMessage = JsonConvert.DeserializeObject<DcpMessage>(msgBody);

                Dictionary<string, Func<IBusinessValidationStrategy>> strategies = getStrategy.GetListOfStrategy(mediator);

                Domain.Model.BusinessValidationProxy businessValidationProxy = await strategies[dcpMessage.ServiceOutput.Items[0].ItemId.ToUpperInvariant()]()
                                .ExecuteStrategy(dcpMessage.ServiceOutput.Items[0].ItemId, dcpMessage.ServiceOutput.Items[0]);

                if (businessValidationProxy.Valid)
                {
                    List<Item> validItems = GetItemCollection(businessValidationProxy.ValidPaths, dcpMessage);

                    if (validItems != null && validItems.Count > 0)
                    {
                        messageProcessResponse = await mediator.Send(new SendServiceOutPutMessageCommand(validItems, message, businessValidationProxy.Valid, businessValidationProxy.ErrorMessages));
                    }
                }

                List<Item> invalidItems = GetItemCollection(businessValidationProxy.InValidPaths, dcpMessage);

                if (invalidItems != null && invalidItems.Count > 0)
                {
                    businessValidationProxy.InValid = false;
                    messageProcessResponse = await mediator.Send(new SendServiceOutPutMessageCommand(invalidItems, message, businessValidationProxy.InValid, businessValidationProxy.ErrorMessages));
                }

                return messageProcessResponse;
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return MessageProcessResponse.Abandon;
            }
        }

        /// <summary>
        /// The OnWait
        /// </summary>


        /// <summary>
        /// Return valid or Invalid item collection
        /// </summary>
        /// <param name="dcpMessage">The DcpMessage<see cref="DcpMessage"/></param>
        /// <param name="paths">collection of path<see cref="List{String}"/></param>
        /// <returns>The <see cref="List{Item}"/></returns>
        private static List<Item> GetItemCollection(List<string> paths, DcpMessage dcpMessage)
        {
            List<Item> items = new List<Item>();
            paths.ForEach(path =>
            {
                items.Add(
                    new Item
                    {
                        ItemId = dcpMessage.ServiceOutput.Items[0].ItemId,
                        ItemPath = path,
                        InlineData = new[] { string.Empty }
                    }
                );
            });

            return items;
        }
    }
}
