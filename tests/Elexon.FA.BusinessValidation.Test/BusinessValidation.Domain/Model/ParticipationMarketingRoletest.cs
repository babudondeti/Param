using Elexon.FA.BusinessValidation.Domain.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Model
{
    public class ParticipantEnergyAssetTest
    {
        [Fact]
        public async Task ParticipantEnergyAsset_Should_Return_TypeOf_ParticipantEnergyAsset()
        {
            await Task.Run(() =>
            {
                ParticipantEnergyAsset participantEnergy = new ParticipantEnergyAsset();
                Assert.NotNull(participantEnergy);
                Assert.IsType<ParticipantEnergyAsset>(participantEnergy);
            });
        }
    }
}
