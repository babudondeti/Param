using Elexon.FA.Core.KeyVault.Abstractions;
using System;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.FPN
{
    public class MockCoreKeyVaultClient : ICoreKeyVaultClient
    {
        public Task<string> GetSecret(string keyName)
        {
            string value = "";
            var result = Task.FromResult<string>(value);
            if (keyName == "elxkvvlongDay")
                result = Task.FromResult<string>(DateTime.Now.ToString());
            else if (keyName == "elxkvvshortDay")
                result = Task.FromResult<string>(DateTime.Now.ToString());
            else if (keyName == "elxkvvsettlementDuration")
                result = Task.FromResult<string>("30");
            else if (keyName == "elxkvvminPairID")
                result = Task.FromResult<string>("-5");
            else if (keyName == "elxkvvmaxPairID")
                result = Task.FromResult<string>("5");
            else if (keyName == "elxkvvBMUreferenceTableURL")
                result = Task.FromResult<string>("BMUreferenceTableURL");
            else if (keyName == "elxkvvStatusTableURL")
                result = Task.FromResult<string>("StatusTableURL");
            else if (keyName == "elxkvvProcessing")
                result = Task.FromResult<string>("Processing");
            else if (keyName == "elxkvvInbound")
                result = Task.FromResult<string>("Inbound");
            else if (keyName == "elxkvvRejected")
                result = Task.FromResult<string>("Rejected");
            return result;
        }
    }
}
