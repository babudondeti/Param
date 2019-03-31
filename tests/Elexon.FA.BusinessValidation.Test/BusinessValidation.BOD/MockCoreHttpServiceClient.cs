using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.Core.HttpClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD
{
    public class MockCoreHttpServiceClient : ICoreHttpServiceClient
    {
        public Task<Bod>GetAsync<Bod>(string url)
        {
            BodMockData data = new BodMockData();
            dynamic result = data.GetBMUParticipant().FirstOrDefault();
            var output = Task.FromResult<List<ParticipantEnergyAsset>>(result);
            return output;
        }
    }
}
