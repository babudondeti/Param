using Elexon.FA.Core.IntegrationMessage.Events;
using Elexon.FA.Core.ServiceBus.Abstractions;
using System;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test
{
    internal class MockSendManager : ISendManager
    {
        public string GetCurrentTopicName(string currentChain)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync<T>(string topicName, T @event) where T : IntegrationEvent
        {
            return Task.CompletedTask;
        }

        public Task SendAsync<T>(string topicName, T @event, System.Collections.Generic.Dictionary<string, dynamic> systemProperties) where T : IntegrationEvent
        {
            return Task.CompletedTask;
        }
    }
}
