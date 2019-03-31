using Elexon.FA.BusinessValidation.Domain.Model;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Model
{
    public class CommonTest
    {
        [Fact]
        public async Task Should_GetProcessingPath_When_ValidationSuccess_GetOutputFolderPath()
        {
            await Task.Run(() =>
            {
                string expectedOutput = "Processing/SAA-I00V-BOD/2018/10/24/29/BOD/BOD.json";
                string inputPath = "Inbound/SAA-I00V-BOD/2018/10/24/29/BOD/BOD.json";
                string settlementDate = "2018-10-24";
                int settlementPeriod = 29;
                string outputBound = "Processing";
                string finalOutput = Common.GetOutputFolderPath(settlementDate, settlementPeriod, inputPath, outputBound);

                Assert.Equal(expectedOutput, finalOutput);
            });
        }
        [Fact]
        public async Task Should_GetRejectedPath_When_ValidationFailure_GetOutputFolderPath()
        {
            await Task.Run(() =>
            {
                string expectedOutput = "Rejected/SAA-I00V-BOD/2018/10/24/29/BOD/BOD.json";
                string inputPath = "Inbound/SAA-I00V-BOD/2018/10/24/29/BOD/BOD.json";
                string settlementDate = "2018-10-24";
                int settlementPeriod = 29;
                string outputBound = "Rejected";
                string finalOutput = Common.GetOutputFolderPath(settlementDate, settlementPeriod, inputPath, outputBound);

                Assert.Equal(expectedOutput, finalOutput);
            });
        }

        [Fact]
        public async Task Common_Should_Return_TypeOf_GetStatusOfBusinessValidation()
        {
            await Task.Run(() =>
            {

               var commonEntity = Common.GetStatusOfBusinessValidation("path");
                Assert.NotNull(commonEntity);
                Assert.IsType<StatusTableEntity>(commonEntity);
            });
        }

        [Fact]
        public async Task Common_Should_Return_TypeOf_GetStatusOfRejectedFile()
        {
            await Task.Run(() =>
            {

                var commonEntity = Common.GetStatusOfRejectedFile("path", "Failure");
                Assert.NotNull(commonEntity);
                Assert.IsType<StatusTableEntity>(commonEntity);
            });
        }
    
    }
}
