using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.Core.HttpClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.FPN
{
    public class MockCoreHttpServiceClient : ICoreHttpServiceClient
    {
        public Task<Fpn> GetAsync<Fpn>(string url)
        {
            FpnMockData data = new FpnMockData();
            dynamic result = data.GetBMUParticipant().FirstOrDefault();
            var output = Task.FromResult<List<ParticipantEnergyAsset>>(result);
            return output;
        }
    }
}
