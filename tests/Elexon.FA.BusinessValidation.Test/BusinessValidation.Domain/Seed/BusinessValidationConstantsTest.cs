namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Seed
{
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="BusinessValidationConstantsTest" />
    /// </summary>
    public class BusinessValidationConstantsTest
    {
        /// <summary>
        /// The Should_Return_CorrectValues_For_Constants
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task Should_Return_CorrectValues_For_Constants()
        {
            await Task.Run(() =>
            {
                //Arrange
                const string expectedErrorCheck = "ErrorCheck";
                const string expectedInbound = "Inbound";
                const string expectedProcessing = "Processing";
                const string expectedRejected = "Rejected";
                const string expectedWarningCheck = "WarningCheck";
                const string expectedFailed = "Failed";
                const string expectedSuccess = "Success";
                const string expectedWarning = "Warning";

                //Act
                var actualErrorCheck = BusinessValidationConstants.ERRORCHECK;
                var actualInbound = BusinessValidationConstants.INBOUND;
                var actualProcessing = BusinessValidationConstants.PROCESSING;
                var actualRejected = BusinessValidationConstants.REJECTED;
                var actualWarningCheck = BusinessValidationConstants.WARNINGCHECK;
                var actualFailed = BusinessValidationConstants.FAILED;
                var actualSuccess = BusinessValidationConstants.SUCCESS;
                var actualWarning = BusinessValidationConstants.WARNING;

                //Assert
                Assert.Equal(actualErrorCheck, expectedErrorCheck);
                Assert.Equal(actualInbound, expectedInbound);
                Assert.Equal(actualProcessing, expectedProcessing);
                Assert.Equal(actualRejected, expectedRejected);
                Assert.Equal(actualWarningCheck, expectedWarningCheck);
                Assert.Equal(actualFailed, expectedFailed);
                Assert.Equal(actualSuccess, expectedSuccess);
                Assert.Equal(actualWarning, expectedWarning);

            });
        }
    }
}
