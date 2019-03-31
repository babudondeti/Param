namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.DISBSADFLOW.Validator
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Elexon.FA.BusinessValidation.DISBSADFlow.Validator;
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.IntegrationMessage;
    using FluentValidation;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="DescribeDisbsadValidator" />
    /// </summary>
    public class DescribeDisbsadValidator
    {
        /// <summary>
        /// Defines the _sut
        /// </summary>
        private readonly DisbsadValidator _sut;

        /// <summary>
        /// Defines the _fixture
        /// </summary>
        private readonly IFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescribeDisbsadValidator"/> class.
        /// </summary>
        public DescribeDisbsadValidator()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _sut = _fixture.Create<DisbsadValidator>();
        }

        /// <summary>
        /// The ItShouldReturnFalseWhenNoRecordExists
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ItShouldReturnFalseWhenNoRecordExists()
        {
            await Task.Run(() =>
            {
                //Arrange 
                Aggregate<Disbsad> aggregate = _fixture.Create<Aggregate<Disbsad>>();
                aggregate.BusinessValidationFlow.Clear();

                //Act
                FluentValidation.Results.ValidationResult result = _sut.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The ItShouldReturnTrueWhenAnyRecordExists
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ItShouldReturnTrueWhenAnyRecordExists()
        {
            await Task.Run(() =>
            {
                //Arrange 
                Aggregate<Disbsad> aggregate = new Aggregate<Disbsad>(new Item(),
                new List<Disbsad> {
                    new Disbsad {SettDate = "2017-02-23",SettlementPeriod = 67, Id = 3},
                    new Disbsad {SettDate = "2017-02-23",SettlementPeriod = 12, Id = 4 }
                }, null, null);


                //Act
                FluentValidation.Results.ValidationResult result = _sut.Validate(aggregate, ruleSet: BusinessValidationConstants.ERRORCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }

        /// <summary>
        /// The ItShouldReturnFalseWhenAnyRecordNotInValidSettlementPeriod
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ItShouldReturnFalseWhenAnyRecordNotInValidSettlementPeriod()
        {
            await Task.Run(() =>
            {
                //Arrange 
                Aggregate<Disbsad> aggregate = new Aggregate<Disbsad>(new Item(),
                new List<Disbsad> {
                new Disbsad {SettDate = "2017-02-23",SettlementPeriod = 67, Id = 3},
                new Disbsad {SettDate = "2017-02-23",SettlementPeriod = 12, Id = 4 }
                }, null, null);

                //Act
                FluentValidation.Results.ValidationResult result = _sut.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.False(result.IsValid);
            });
        }

        /// <summary>
        /// The ItShouldReturnTrueWhenAllRecordInValidSettlementPeriod
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        [Fact]
        public async Task ItShouldReturnTrueWhenAllRecordInValidSettlementPeriod()
        {
            await Task.Run(() =>
            {
                //Arrange 
                Aggregate<Disbsad> aggregate = new Aggregate<Disbsad>(new Item(),
                new List<Disbsad> {
                    new Disbsad { SettDate = "2017-02-23", SettlementPeriod = 17, Id = 3,Soflag="true",StorFlag="true"},
                    new Disbsad { SettDate = "2017-02-23", SettlementPeriod = 24, Id = 4, Soflag="true",StorFlag="true" }
                },
                null, null);

                //Act
                FluentValidation.Results.ValidationResult result = _sut.Validate(aggregate, ruleSet: BusinessValidationConstants.WARNINGCHECK);

                //Assert
                Assert.True(result.IsValid);
            });
        }
    }
}
