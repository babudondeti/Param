namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.NETBSADFLOW.Validator
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.BusinessValidation.NETBSADFlow.Validator;
    using Elexon.FA.Core.IntegrationMessage;
    using FluentValidation;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// Defines the <see cref="DescribeNetbsadValidator" />
    /// </summary>
    public class DescribeNetbsadValidator
    {
        /// <summary>
        /// Defines the _sut
        /// </summary>
        private NetbsadValidator _sut;

        /// <summary>
        /// Defines the _fixture
        /// </summary>
        private IFixture _fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescribeNetbsadValidator"/> class.
        /// </summary>
        public DescribeNetbsadValidator()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _sut = _fixture.Create<NetbsadValidator>();
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
                Aggregate<Netbsad> aggregate = _fixture.Create<Aggregate<Netbsad>>();
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
                Aggregate<Netbsad> aggregate = _fixture.Create<Aggregate<Netbsad>>();

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
                Aggregate<Netbsad> aggregate = new Aggregate<Netbsad>(new Item(),
                new List<Netbsad> {
                new Netbsad {SettDate = "2017-02-23",SettlementPeriod = 67 },
                new Netbsad {SettDate = "2017-02-23",SettlementPeriod = 12 }
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
                Aggregate<Netbsad> aggregate = new Aggregate<Netbsad>(new Item(),
                new List<Netbsad> {
                    new Netbsad { SettDate = "2017-02-23", SettlementPeriod = 17 },
                    new Netbsad { SettDate = "2017-02-23", SettlementPeriod = 24 }
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
