namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.FPN.Validator
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Query;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.BusinessValidation.FPNFlow.Validator;
    using Elexon.FA.Core.IntegrationMessage;
    using FluentValidation;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="FpnValidatorTest" />
    /// </summary>
    public class FpnValidatorTest : IDisposable
    {
        /// <summary>
        /// Defines the _mockData
        /// </summary>
        private FpnMockData _mockData;

        /// <summary>
        /// Defines the _mockQuery
        /// </summary>
        internal Mock<IQueryFlow<Fpn>> _mockQuery = new Mock<IQueryFlow<Fpn>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FpnValidatorTest"/> class.
        /// </summary>
        public FpnValidatorTest()
        {
            _mockData = new FpnMockData();
        }
        /// <summary>
        /// Dispose unused resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// The Clean up
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            _mockData = null;
        }
        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_SettlementDay_Is_Valid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_SettlementDay_Is_Valid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_SettlementDay_Is_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_SettlementDay_Is_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 12, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 12, 10, 1, 30, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_SettlementPeriod_Is_Valid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_SettlementPeriod_Is_Valid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_SettlementPeriod_Is_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_SettlementPeriod_Is_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 40, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_NoRecordExists
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_NoRecordExists()
        {
            await Task.Run(() =>
            {
                //Arrange

                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };
                aggregate.BusinessValidationFlow.Clear();
                FpnValidator validator = new FpnValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_RecordExists
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_RecordExists()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };
                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_FileAlreadyNotExists
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_FileAlreadyNotExists()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };
                string blobName = "Processing/SAA-I003-FPN/2018/10/24/29/FPN/PN.json";
                FpnValidator validator = new FpnValidator();
                _mockQuery.Setup(s => s.ExistsAsync(blobName)).Returns(Task.FromResult(true));
                aggregate.FileAlreadyExistOrNot = false;
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_FileAlreadyExists
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_FileAlreadyExists()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };
                string blobName = "Processing/SAA-I003-FPN/2018/11/10/3/FPN/PN.json";
                FpnValidator validator = new FpnValidator();
                _mockQuery.Setup(s => s.ExistsAsync(blobName)).Returns(Task.FromResult(true));
                aggregate.FileAlreadyExistOrNot = true;
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_BMUUnit_Is_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_BMUUnit_Is_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().BmuName = "ABCD123";
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.MSG_INVALIDBMU);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_BMUUnit_Is_Valid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_BMUUnit_Is_Valid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().BmuName = "GTYPE150";
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_TimeFrom_IsGreaterThan_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_TimeFrom_IsGreaterThan_TimeTo()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = new List<Fpn>();
                Fpn fpn = _mockData.GetFpns().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpn.TimeFrom = new DateTime(2018, 11, 10, 1, 30, 00);
                fpn.TimeTo = new DateTime(2018, 11, 10, 1, 00, 00);
                fpns.Add(fpn);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.MSG_TOTIMEGREATERTHANFROMTIME);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_TimeFrom_IsSmallerThan_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_TimeFrom_IsSmallerThan_TimeTo()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = new List<Fpn>();
                Fpn fpn = _mockData.GetFpns().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpn.TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpn.TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                fpns.Add(fpn);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_TimeFrom_NotThereFor_SameBmUnitRecord
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_TimeFrom_NotThereFor_SameBmUnitRecord()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = new List<Fpn>();
                Fpn fpn = _mockData.GetFpns().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpn.TimeFrom = new DateTime(2018, 11, 10, 1, 15, 00);
                fpn.TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                fpns.Add(fpn);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.MSG_STARTTIMETHEREFORSAMEBMUNITRECORD);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_TimeFrom_ThereFor_SameBmUnitRecord
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_TimeFrom_ThereFor_SameBmUnitRecord()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = new List<Fpn>();
                Fpn fpn = _mockData.GetFpns().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpn.TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpn.TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                fpns.Add(fpn);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_TimeTo_NotThereFor_SameBmUnitRecord
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_TimeTo_NotThereFor_SameBmUnitRecord()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = new List<Fpn>();
                Fpn fpn = _mockData.GetFpns().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpn.TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpn.TimeTo = new DateTime(2018, 11, 10, 1, 25, 00);
                fpns.Add(fpn);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.MSG_SENDTIMETHEREFORSAMEBMUNITRECORD);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_TimeTo_ThereFor_SameBmUnitRecord
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_TimeTo_ThereFor_SameBmUnitRecord()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = new List<Fpn>();
                Fpn fpn = _mockData.GetFpns().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpn.TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                fpn.TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                fpns.Add(fpn);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_TimeFrom_IsNot_EqualTo_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_TimeFrom_IsNot_EqualTo_TimeTo()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns().ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 05, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 15, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);

            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_TimeFrom_Is_EqualTo_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_TimeFrom_Is_EqualTo_TimeTo()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns().ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);

            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_False_When_TimeFrom_AndTimeTo_OverLaps_WithNextRecord_SameBMUnit
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_False_When_TimeFrom_AndTimeTo_OverLaps_WithNextRecord_SameBMUnit()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns().ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                fpns.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 10, 00);
                fpns.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 20, 00);
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);

            });
        }

        /// <summary>
        /// The FpnValidator_Validate_Should_Return_True_When_TimeFrom_AndTimeTo_Not_OverLaps_WithNextRecord_SameBMUnit
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task FpnValidator_Validate_Should_Return_True_When_TimeFrom_AndTimeTo_Not_OverLaps_WithNextRecord_SameBMUnit()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Fpn> fpns = _mockData.GetFpns().ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                Aggregate<Fpn> aggregate = new Aggregate<Fpn>(new Item(), fpns, bmuUnit, null);
                FpnValidator validator = new FpnValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);

            });
        }
    }
}
