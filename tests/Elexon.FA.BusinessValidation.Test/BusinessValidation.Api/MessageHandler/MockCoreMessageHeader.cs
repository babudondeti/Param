using Elexon.FA.Core.IntegrationMessage;
using Elexon.FA.Core.MessageHeader.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test
{
    public class MockCoreMessageHeader : ICoreMessageHeader
    {
        public Task<Dictionary<string, object>> GetMicroServiceDataItemsAsync(CoreMessage coreMessage)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SendServiceOutputToTopicAsync(List<Item> items, CoreMessage coreMessage)
        {
            throw new System.NotImplementedException();
        }

    }
}
