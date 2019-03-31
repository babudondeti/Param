using Elexon.FA.BusinessValidation.Domain.Model;
using Elexon.FA.Core.HttpClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF
{
    public class MockCoreHttpServiceClient : ICoreHttpServiceClient
    {
        public Task<Boalf>GetAsync<Boalf>(string url)
        {
            BoalfMockData data = new BoalfMockData();
            dynamic result = data.GetBoalfs().FirstOrDefault();
            var output = Task.FromResult<List<ParticipantEnergyAsset>>(result);
            return output;
        }
    }
}
