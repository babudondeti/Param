namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD.Validator
{
    using Elexon.FA.BusinessValidation.BODFlow.Validator;
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.IntegrationMessage;
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="BodValidatorTest" />
    /// </summary>
    public class BodValidatorTest : IDisposable
    {
        /// <summary>
        /// Defines the _mockData
        /// </summary>
        private BodMockData _mockData;

        /// <summary>
        /// Initializes a new instance of the <see cref="BodValidatorTest"/> class.
        /// </summary>
        public BodValidatorTest()
        {
            _mockData = new BodMockData();
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
        /// The BodValidator_Validate_Should_Return_True_When_No_Record_To_Validate
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_No_Record_To_Validate()
        {

            await Task.Run(() =>
            {
                List<Bod> bods = new List<Bod>();
                List<ParticipantEnergyAsset> participantEnergyAssets = new List<ParticipantEnergyAsset>();
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, participantEnergyAssets, null);
                BodValidator validator = new BodValidator();

                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate);

                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_TimeFrom_IsGreaterThan_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_TimeFrom_IsGreaterThan_TimeTo()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = new List<Bod>();
                Bod bod = _mockData.GetBods().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bod.TimeFrom = new DateTime(2018, 11, 10, 0, 30, 00);
                bod.TimeTo = new DateTime(2018, 11, 10, 0, 00, 00);
                bods.Add(bod);
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null);
                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.MSG_TIMEFROMGREATERTHANTIMETO);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_TimeFrom_IsSmallerThan_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_TimeFrom_IsSmallerThan_TimeTo()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = new List<Bod>();
                Bod bod = _mockData.GetBods().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bod.TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00);
                bod.TimeTo = new DateTime(2018, 11, 10, 0, 30, 00);
                bods.Add(bod);
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null);
                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_SettlementDay_Is_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_SettlementDay_Is_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods.FirstOrDefault().TimeFrom = new DateTime(2018, 12, 10, 0, 00, 00);
                bods.FirstOrDefault().TimeTo = new DateTime(2018, 12, 10, 0, 30, 00);
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.MSG_INVALIDSETTLEMENTDAY);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_SettlementDay_Is_Valid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_SettlementDay_Is_Valid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00);
                bods.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 0, 30, 00);
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_SettlementPeriod_Is_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_SettlementPeriod_Is_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00);
                bods.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 1, 30, 00);
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.MSG_INVALIDSETTLEMENTDAY);
                Assert.Equal(result.Errors[1].ToString(), BusinessValidationConstants.MSG_INVALIDSETTLEMENTPERIOD);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_SettlementPeriod_Is_Valid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_SettlementPeriod_Is_Valid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods.FirstOrDefault().TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00);
                bods.FirstOrDefault().TimeTo = new DateTime(2018, 11, 10, 0, 30, 00);
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_BMUUnit_Is_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_BMUUnit_Is_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods.FirstOrDefault().BmuName = "ABCD123";
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

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
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods.FirstOrDefault().BmuName = "GTYPE150";
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_PairId_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_PairId_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -6;
                bods[1].PairId = 0;
                bods[6].PairId = 6;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(bods[0].PairId, aggregate.InValidFlow[0].PairId);
                Assert.Equal(bods[1].PairId, aggregate.InValidFlow[1].PairId);
                Assert.Equal(bods[6].PairId, aggregate.InValidFlow[2].PairId);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_PairId_Valid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_PairId_Valid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -5;
                bods[1].PairId = -4;
                bods[6].PairId = 2;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
                Assert.Equal(10, aggregate.ValidFlow.Distinct().Count());
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_PairId_LevelToAndFrom_Have_Different_Sign
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_PairId_LevelToAndFrom_Have_Different_Sign()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -5;
                bods[0].LevelFrom = 2;
                bods[0].LevelTo = 6;
                bods[5].PairId = 1;
                bods[5].LevelFrom = -2;
                bods[5].LevelTo = -6;
                bods[9].PairId = 0;
                bods[9].LevelFrom = -2;
                bods[9].LevelTo = -6;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(3, aggregate.InValidFlow.Distinct().Count());
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_PairId_LevelToAndFrom_Have_Same_Sign
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_PairId_LevelToAndFrom_Have_Same_Sign()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -5;
                bods[0].LevelFrom = -2;
                bods[0].LevelTo = -6;
                bods[5].PairId = 1;
                bods[5].LevelFrom = 2;
                bods[5].LevelTo = 6;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
                Assert.Empty(aggregate.InValidFlow.Distinct());
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_PairId_AreNot_Consecutive_For_Particular_BMUUnit
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_PairId_AreNot_Consecutive_For_Particular_BMUUnit()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -5;
                bods[1].PairId = -4;
                bods[2].PairId = -4;
                bods[3].PairId = -2;
                bods[4].PairId = -1;
                bods[5].PairId = 1;
                bods[6].PairId = 3;
                bods[7].PairId = 4;
                bods[8].PairId = 5;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_PairId_Are_Consecutive_For_Particular_BMUUnit
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_PairId_Are_Consecutive_For_Particular_BMUUnit()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -5;
                bods[1].PairId = -4;
                bods[2].PairId = -3;
                bods[3].PairId = -2;
                bods[4].PairId = -1;
                bods[5].PairId = 1;
                bods[6].PairId = 2;
                bods[7].PairId = 3;
                bods[8].PairId = 4;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
                Assert.Equal(10, aggregate.ValidFlow.Count);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_False_When_Bods_Has_Duplicate_Record
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_False_When_Bods_Has_Duplicate_Record()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -4;
                bods[1].PairId = -4;
                bods[2].PairId = -3;
                bods[3].PairId = -2;
                bods[4].PairId = -1;
                bods[5].PairId = 1;
                bods[6].PairId = 2;
                bods[7].PairId = 3;
                bods[8].PairId = 3;
                bods[9].PairId = 4;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The BodValidator_Validate_Should_Return_True_When_Bods_Has_No_Duplicate_Record
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BodValidator_Validate_Should_Return_True_When_Bods_Has_No_Duplicate_Record()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Bod> bods = _mockData.GetBods();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                bods[0].PairId = -5;
                bods[1].PairId = -4;
                bods[2].PairId = -3;
                bods[3].PairId = -2;
                bods[4].PairId = -1;
                bods[5].PairId = 1;
                bods[6].PairId = 2;
                bods[7].PairId = 3;
                bods[8].PairId = 4;
                bods[9].PairId = 5;
                Aggregate<Bod> aggregate = new Aggregate<Bod>(new Item(), bods, bmuUnit, null)
                {
                    SettlementDuration = 30,
                    MinPairId = -5,
                    MaxPairId = 5
                };

                BodValidator validator = new BodValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }
    }
}
