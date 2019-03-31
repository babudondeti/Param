using Elexon.FA.Core.ServiceBus.Abstractions;
using Elexon.FA.Core.ServiceBus.Common;
using System;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test
{
    internal class MockReceiveManager : IReceiveManager
    {
        public Task Receive<T>(Func<T, Task<MessageProcessResponse>> onProcess, Action<Exception> onError)
        {
            return Task.CompletedTask;
        }
    }
}
