using Elexon.FA.Core.KeyVault.Abstractions;
using System;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.DISBSADFLOW
{
    public class MockCoreKeyVaultClient : ICoreKeyVaultClient
    {
        public Task<string> GetSecret(string keyName)
        {
            string value = "";
            var result = Task.FromResult<string>(value);
            if (keyName == "longDay")
                result = Task.FromResult<string>(DateTime.Now.ToString());
            else if (keyName == "shortDay")
                result = Task.FromResult<string>(DateTime.Now.ToString());
            else if (keyName == "settlementDuration")
                result = Task.FromResult<string>("30");
            else if (keyName == "minPairID")
                result = Task.FromResult<string>("-5");
            else if (keyName == "maxPairID")
                result = Task.FromResult<string>("5");
            else if (keyName == "BMUreferenceTableURL")
                result = Task.FromResult<string>("BMUreferenceTableURL");
            else if (keyName == "StatusTableURL")
                result = Task.FromResult<string>("StatusTableURL");
            else if (keyName == "Processing")
                result = Task.FromResult<string>("Processing");
            else if (keyName == "Inbound")
                result = Task.FromResult<string>("Inbound");
            else if (keyName == "Rejected")
                result = Task.FromResult<string>("Rejected");
            return result;
        }
    }
}
