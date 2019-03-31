namespace Elexon.FA.BusinessValidation.Api.MessageHandlers
{
    using Elexon.FA.Core.IntegrationMessage;
    using Elexon.FA.Core.ServiceBus.Common;
    using MediatR;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SendServiceOutPutMessageCommand" />
    /// </summary>
    public class SendServiceOutPutMessageCommand : IRequest<MessageProcessResponse>
    {
        /// <summary>
        /// Gets or sets the _items
        /// </summary>
        public List<Item> Items { get; }

        /// <summary>
        /// Gets or sets the _message
        /// </summary>
        public CoreMessage Message { get; }

        public bool Status { get; }

        public List<string> ErrorMessage { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SendServiceOutPutMessageCommand"/> class.
    /// </summary>
    /// <param name="items">The items<see cref="List{Item}"/></param>
    /// <param name="message">The message<see cref="CoreMessage"/></param>
    public SendServiceOutPutMessageCommand(List<Item> items, CoreMessage message, bool status, List<string> errorMessage)
        {
            Items = items;
            Message = message;
            Status = status;
            ErrorMessage = errorMessage;
        }
    }
}
