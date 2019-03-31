namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF.Validator
{
    using Elexon.FA.BusinessValidation.BOALFFlow.Validator;
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
    /// Defines the <see cref="BoalfValidatorTest" />
    /// </summary>
    public class BoalfValidatorTest : IDisposable
    {
        /// <summary>
        /// Defines the _mockData
        /// </summary>
        private BoalfMockData _mockData;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoalfValidatorTest"/> class.
        /// </summary>
        public BoalfValidatorTest()
        {
            _mockData = new BoalfMockData();
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
        /// The BoalfValidator_Validate_Should_Return_True_When_No_Record_To_Validate
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_No_Record_To_Validate()
        {
            await Task.Run(() =>
            {
                List<Boalf> boalfs = new List<Boalf>();
                List<ParticipantEnergyAsset> participantEnergyAsset = new List<ParticipantEnergyAsset>();
                List<BoalfIndexTable> updateOrInsFlow = new List<BoalfIndexTable>();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalfs, participantEnergyAsset, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate);
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_TimeFrom_IsLessThan_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_TimeFrom_IsLessThan_TimeTo()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalfs = new List<Boalf>();
                Boalf boalf = _mockData.GetBoalfs().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                boalf.TimeFrom = new DateTime(2018, 04, 09, 14, 00, 00);
                boalf.TimeTo = new DateTime(2018, 04, 09, 14, 30, 00);
                boalfs.Add(boalf);
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalfs, bmuUnit, updateOrInsFlow);
                BoalfValidator Validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = Validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_When_TimeFrom_IsGreaterThan_TimeTo
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_When_TimeFrom_IsGreaterThan_TimeTo()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalfs = new List<Boalf>();
                Boalf boalf = _mockData.GetBoalfs().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                boalf.TimeFrom = new DateTime(2018, 11, 10, 0, 30, 00);
                boalf.TimeTo = new DateTime(2018, 11, 10, 0, 00, 00);
                boalfs.Add(boalf);
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalfs, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();

                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.MSG_TIMEFROMGREATERTHANTIMETO);

            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_TimeFrom_And_TimeTo_HaveLessThanOr_EqualtoFourHoursDifferenc
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_TimeFrom_And_TimeTo_HaveLessThanOr_EqualtoFourHoursDifferenc()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalfs = new List<Boalf>();
                Boalf boalf = _mockData.GetBoalfs().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                boalf.TimeFrom = new DateTime(2018, 04, 09, 00, 00, 00);
                boalf.TimeTo = new DateTime(2018, 04, 09, 04, 00, 00);
                boalfs.Add(boalf);
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalfs, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_False_When_TimeFrom_And_TimeTo_HaveGreaterThanFourHoursDifference
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_False_When_TimeFrom_And_TimeTo_HaveGreaterThanFourHoursDifference()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalfs = new List<Boalf>();
                Boalf boalf = _mockData.GetBoalfs().FirstOrDefault();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                boalf.TimeFrom = new DateTime(2018, 11, 10, 00, 00, 00);
                boalf.TimeTo = new DateTime(2018, 11, 10, 05, 00, 00);
                boalfs.Add(boalf);
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalfs, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.BOALF_MSG_FROMTIMEANDTOTIMEHAVEONLYFOURHOURDIFFERENCE);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_ForSameBMUUnitAndBidOfferAcceptanceNumber_HaveSameAcceptanceTime
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_ForSameBMUUnitAndBidOfferAcceptanceNumber_HaveSameAcceptanceTime()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(1).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_False_When_ForSameBMUUnitAndBidOfferAcceptanceNumber_HaveDifferentAcceptanceTime
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_False_When_ForSameBMUUnitAndBidOfferAcceptanceNumber_HaveDifferentAcceptanceTime()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(1).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                boalf[0].AcceptanceTime = new DateTime(2018, 04, 09, 14, 30, 00);
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.BOALF_MSG_DIFFERENTACCEPTANCETIME);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameDeemedBidOfferFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameDeemedBidOfferFlag()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(1).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentDeemedBidOfferFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentDeemedBidOfferFlag()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(3).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.BOALF_MSG_DIFFERENTDEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_AMMENDMENTFLAG_OR_STORFLAG);

            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameSoFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameSoFlag()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(5).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentSoFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentSoFlag()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(7).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.BOALF_MSG_DIFFERENTDEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_AMMENDMENTFLAG_OR_STORFLAG);

            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameAmmendentFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameAmmendentFlag()
        {

            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(9).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentAmmendentFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentAmmendentFlag()
        {

            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(11).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.LastOrDefault().ToString(), BusinessValidationConstants.BOALF_MSG_DIFFERENTDEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_AMMENDMENTFLAG_OR_STORFLAG);

            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameStoreFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveSameStoreFlag()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(13).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentStoreFlag
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_False_When_SameBMUNameBidOfferAcceptanceNumberAcceptanceTime_HaveDifferentStoreFlag()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(15).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.BOALF_MSG_DIFFERENTDEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_AMMENDMENTFLAG_OR_STORFLAG);

            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_True_When_TimeFromOfPreviousRecordEqualToTimetoOfNextRecord
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_True_When_TimeFromOfPreviousRecordEqualToTimetoOfNextRecord()
        {

            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(17).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The boalfValidator_Validate_Should_Return_False_When_TimeFromOfPreviousRecordNotToTimetoOfNextRecord
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task boalfValidator_Validate_Should_Return_False_When_TimeFromOfPreviousRecordNotToTimetoOfNextRecord()
        {
            await Task.Run(() =>
            {

                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(19).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors.FirstOrDefault().ToString(), BusinessValidationConstants.BOALF_MSG_DIFFERENTTIMETOOFPREVIOUSRECORDANDTIMEFROMOFNEXTRECORD);

            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_True_When_BMUUnit_Is_Valid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_BMUUnit_Is_Valid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(21).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_When_BMUUnit_Is_InValid
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_When_BMUUnit_Is_InValid()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(23).Take(2).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> updateOrInsFlow = _mockData.GetUpdateorINSFlow();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, updateOrInsFlow);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.MSG_INVALIDBMU);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagORI_ThereShouldNotberecordreceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagORI_ThereShouldNotberecordreceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(25).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagORI_ThereShouldberecordreceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagORI_ThereShouldberecordreceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(26).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(1).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.BOALF_MSG_AMMENDMENTFLAG_ORI_OR_INS);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagDel_ThereShouldberecordReceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagDel_ThereShouldberecordReceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(27).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(2).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagDel_ThereShouldNotbeRecordReceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagDel_ThereShouldNotbeRecordReceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(28).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(3).Take(1).ToList();
                boalfIndexTables.FirstOrDefault().PartitionKey = "Different";
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.BOALF_MSG_AMMENDMENTFLAG_UPD_OR_DEL);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagINS_ThereShouldNotberecordreceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagINS_ThereShouldNotberecordreceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(29).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(4).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagINS_ThereShouldberecordreceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagINS_ThereShouldberecordreceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(30).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(5).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.BOALF_MSG_AMMENDMENTFLAG_ORI_OR_INS);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagUPD_ThereShouldberecordReceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_ForParticularBMUnitBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagUPD_ThereShouldberecordReceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(31).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(6).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_ForParticularBMUniBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagUPD_ThereShouldNotbeRecordReceivedEarlier
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_ForParticularBMUniBoAcceptanceNumberBoAcceptanceTimeIfAmendmentFlagUPD_ThereShouldNotbeRecordReceivedEarlier()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(32).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(7).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Assert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.BOALF_MSG_AMMENDMENTFLAG_UPD_OR_DEL);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_True_When_AmendmentFlagIsORIOrINSOrUPDOrDEL
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_AmendmentFlagIsORIOrINSOrUPDOrDEL()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(32).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                boalf.FirstOrDefault().AmendmentFlag = "False";
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(7).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Asert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_When_AmendmentFlagIsNotORIOrINSOrUPDOrDEL
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_When_AmendmentFlagIsNotORIOrINSOrUPDOrDEL()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(32).Take(1).ToList();
                boalf.FirstOrDefault().AmendmentFlag = BusinessValidationConstants.ISFALSE;
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(7).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Asert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.BOALF_MSG_AMENDMENTFLAGNOTHAVEORI_OR_INS_OR_DEL_OR_UPD);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_True_When_IsDeemedBidOfferFlagOrSoFlagOrStorFlaghaveOnlyTrueOrFalse
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_True_When_IsDeemedBidOfferFlagOrSoFlagOrStorFlaghaveOnlyTrueOrFalse()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(32).Take(1).ToList();
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                boalf.FirstOrDefault().DeemedBidOfferFlag = "UPD";
                boalf.FirstOrDefault().StorFlag = "Dif";
                boalf.FirstOrDefault().SoFlag = "DEL";
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(7).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Asert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The BoalfValidator_Validate_Should_Return_False_When_IsDeemedBidOfferFlagOrSoFlagOrStorFlagNothaveTrueOrFalse
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task BoalfValidator_Validate_Should_Return_False_When_IsDeemedBidOfferFlagOrSoFlagOrStorFlagNothaveTrueOrFalse()
        {
            await Task.Run(() =>
            {
                //Arrange
                List<Boalf> boalf = _mockData.GetBoalfs().Skip(32).Take(1).ToList();
                boalf.FirstOrDefault().DeemedBidOfferFlag = BusinessValidationConstants.UPDATEORINS;
                boalf.FirstOrDefault().StorFlag = BusinessValidationConstants.AMMENDMENTFLAGUPD;
                boalf.FirstOrDefault().SoFlag = BusinessValidationConstants.AMMENDMENTFLAGDEL;
                List<ParticipantEnergyAsset> bmuUnit = _mockData.GetBMUParticipant();
                List<BoalfIndexTable> boalfIndexTables = _mockData.GetUpdateorINSFlow().Skip(7).Take(1).ToList();
                Aggregate<Boalf> aggregate = new Aggregate<Boalf>(new Item(), boalf, bmuUnit, boalfIndexTables);
                BoalfValidator validator = new BoalfValidator();
                //Act
                FluentValidation.Results.ValidationResult result = validator.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);
                //Asert
                Assert.False(result.IsValid);
                Assert.Equal(result.Errors[0].ToString(), BusinessValidationConstants.BOALF_MSG_DEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_STORFLAGSHOULDBETRUEORFALSE);
            });
        }
    }
}
