using Elexon.FA.BusinessValidation.Domain.Seed;
using Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.Domain.Seed
{
    public class ExtensionMethodTest
    {
        [Fact]
        public async Task Should_GetSettlementPeriod_When_Passing_TimeTo_And_TimeFrom_1()
        {
            await Task.Run(() =>
            {
                //Arrange
                var timeFrom = new DateTime(2018, 11, 10, 0, 00, 00);
                var timeTo = new DateTime(2018, 11, 10, 0, 30, 00);
                //Act
                var actualSettlementPeriod = ExtensionMethod.GetSettlementPeriod(timeTo, timeFrom);
                var expectedSettlementPeriod = 1;
                //Assert
                Assert.Equal(expectedSettlementPeriod, actualSettlementPeriod);
            });
        }

        [Fact]
        public async Task Should_GetSettlementPeriod_When_Passing_TimeTo_And_TimeFrom_4()
        {
            await Task.Run(() =>
            {
                //Arrange
                var timeFrom = new DateTime(2018, 11, 10, 2, 00, 00);
                var timeTo = new DateTime(2018, 11, 10, 2, 30, 00);
                //Act
                var actualSettlementPeriod = ExtensionMethod.GetSettlementPeriod(timeTo, timeFrom);
                var expectedSettlementPeriod = 5;
                //Assert
                Assert.Equal(expectedSettlementPeriod, actualSettlementPeriod);
            });
        }

        [Fact]
        public async Task Should_GetSettlementPeriodForBOALF_When_Passing_TimeTo_And_TimeFrom_1()
        {
            await Task.Run(() =>
            {
                //Arrange
                var timeFrom = new DateTime(2018, 11, 10, 0, 00, 00);
                var timeTo = new DateTime(2018, 11, 10, 0, 30, 00);
                //Act
                var actualSettlementPeriod = ExtensionMethod.GetSettlementPeriodForBv(timeTo, timeFrom);
                var expectedSettlementPeriod = 1;
                //Assert
                Assert.Equal(expectedSettlementPeriod, actualSettlementPeriod);
            });
        }

        [Fact]
        public async Task Should_GetSettlementPeriodForBOALF_When_Passing_TimeTo_And_TimeFrom_4()
        {
            await Task.Run(() =>
            {
                //Arrange
                var timeFrom = new DateTime(2018, 11, 10, 2, 00, 00);
                var timeTo = new DateTime(2018, 11, 10, 2, 30, 00);
                //Act
                var actualSettlementPeriod = ExtensionMethod.GetSettlementPeriodForBv(timeTo, timeFrom);
                var expectedSettlementPeriod = 5;
                //Assert
                Assert.Equal(expectedSettlementPeriod, actualSettlementPeriod);
            });
        }

        [Fact]
        public async Task Should_GetSettlementPeriodStartTime_For_GivenSettlementPeriod()
        {
            await Task.Run(() =>
            {
                //Arrange
                var settlementPeriod = 3;
                //Act
                var actualSettlementPeriodStartTime = ExtensionMethod.SettlementPeriodStartTime(settlementPeriod);
                var expectedSettlementPeriodStartTime = "01:00:00";
                //Assert
                Assert.Equal(expectedSettlementPeriodStartTime, actualSettlementPeriodStartTime);
            });
        }

        [Fact]
        public async Task Should_GetSettlementPeriodEndTime_For_GivenSettlementPeriod()
        {
            await Task.Run(() =>
            {
                //Arrange
                var settlementPeriod = 3;
                //Act
                var actualSettlementPeriodEndTime = ExtensionMethod.SettlementPeriodEndTime(settlementPeriod);
                var expectedSettlementPeriodEndTime = "01:30:00";
                //Assert
                Assert.Equal(expectedSettlementPeriodEndTime, actualSettlementPeriodEndTime);
            });
        }

        [Fact]

        public async Task Should_Return_False_When_List_IsConsecutive_IsEmpty()
        {
            await Task.Run(() =>
            {
                List<Int32> consecutiveList = new List<Int32>
            {
                new Int32
                {

                },
                new Int32
                {

                },
                new Int32
                {

                }

                };
                bool actualListCheckForConsecutive = ExtensionMethod.IsConsecutive(consecutiveList);
                bool expectedListCheckForConsecutive = false;
                //Assert
                Assert.Equal(expectedListCheckForConsecutive, actualListCheckForConsecutive);
            });
        }

        [Fact]
        public void GetMultipleSPForSpanningRecord_Should_Return_SP_23_And_24()
        {
            var mockData = new BoalfMockData();
            var boalf = mockData.GetBoalfs().FirstOrDefault();
            boalf.TimeFrom = new DateTime(2018, 04, 09, 11, 28, 00);
            boalf.TimeTo = new DateTime(2018, 04, 09, 11, 32, 00);

            var result = boalf.TimeTo.GetMultipleSpForSpanningRecord(boalf.TimeFrom, 30);

            Assert.Equal(23, result[0].SettlementPeriod);
            Assert.Equal(24, result[1].SettlementPeriod);
        }

        [Fact]
        public void GetMultipleSPForSpanningRecord_Should_Return_SP_23_24_25_And_26()
        {
            var mockData = new BoalfMockData();
            var boalf = mockData.GetBoalfs().FirstOrDefault();
            boalf.TimeFrom = new DateTime(2018, 04, 09, 11, 28, 00);
            boalf.TimeTo = new DateTime(2018, 04, 09, 12, 32, 00);

            var result = boalf.TimeTo.GetMultipleSpForSpanningRecord(boalf.TimeFrom, 30);

            Assert.Equal(23, result[0].SettlementPeriod);
            Assert.Equal(24, result[1].SettlementPeriod);
            Assert.Equal(25, result[2].SettlementPeriod);
            Assert.Equal(26, result[3].SettlementPeriod);
        }

        [Fact]
        public void GetMultipleSPForSpanningRecord_Should_Return_SP_23_24_25_26_27_And_28()
        {
            BoalfMockData mockData = new BoalfMockData();
            var boalf = mockData.GetBoalfs().FirstOrDefault();
            boalf.TimeFrom = new DateTime(2018, 04, 09, 11, 28, 00);
            boalf.TimeTo = new DateTime(2018, 04, 09, 13, 32, 00);

            var result = boalf.TimeTo.GetMultipleSpForSpanningRecord(boalf.TimeFrom, 30);

            Assert.Equal(23, result[0].SettlementPeriod);
            Assert.Equal(24, result[1].SettlementPeriod);
            Assert.Equal(25, result[2].SettlementPeriod);
            Assert.Equal(26, result[3].SettlementPeriod);
            Assert.Equal(27, result[4].SettlementPeriod);
            Assert.Equal(28, result[5].SettlementPeriod);
        }

        [Fact]
        public void GetMultipleSPForSpanningRecord_Should_Return_SP_3_4_and_5()
        {
            BoalfMockData mockData = new BoalfMockData();
            var boalf = mockData.GetBoalfs().FirstOrDefault();
            boalf.TimeFrom = new DateTime(2018, 04, 09, 1, 15, 00);
            boalf.TimeTo = new DateTime(2018, 04, 09, 2, 10, 00);

            var result = boalf.TimeTo.GetMultipleSpForSpanningRecord(boalf.TimeFrom, 30);

            Assert.Equal(3, result[0].SettlementPeriod);
            Assert.Equal(4, result[1].SettlementPeriod);
            Assert.Equal(5, result[2].SettlementPeriod);
        }

        [Fact]
        public void GetMultipleSPForSpanningRecord_Should_Return_SP_47_48_1_and_2()
        {
            BoalfMockData mockData = new BoalfMockData();
            var boalf = mockData.GetBoalfs().FirstOrDefault();
            boalf.TimeFrom = new DateTime(2018, 04, 09, 23, 15, 00);
            boalf.TimeTo = new DateTime(2018, 04, 10, 00, 32, 00);

            var result = boalf.TimeTo.GetMultipleSpForSpanningRecord(boalf.TimeFrom, 30);

            Assert.Equal(47, result[0].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 09, 00, 00, 00), result[0].SettlementDay);
            Assert.Equal(48, result[1].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 09, 00, 00, 00), result[1].SettlementDay);
            Assert.Equal(1, result[2].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 10, 00, 00, 00), result[2].SettlementDay);
            Assert.Equal(2, result[3].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 10, 00, 00, 00), result[3].SettlementDay);
        }

        [Fact]
        public void GetMultipleSPForSpanningRecord_Should_Return_SP_47_48_1_2_3_and_4()
        {
            BoalfMockData mockData = new BoalfMockData();
            var boalf = mockData.GetBoalfs().FirstOrDefault();
            boalf.TimeFrom = new DateTime(2018, 04, 09, 23, 00, 00);
            boalf.TimeTo = new DateTime(2018, 04, 10, 2, 00, 00);

            var result = boalf.TimeTo.GetMultipleSpForSpanningRecord(boalf.TimeFrom, 30);

            Assert.Equal(47, result[0].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 09, 00, 00, 00), result[0].SettlementDay);
            Assert.Equal(48, result[1].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 09, 00, 00, 00), result[1].SettlementDay);
            Assert.Equal(1, result[2].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 10, 00, 00, 00), result[2].SettlementDay);
            Assert.Equal(2, result[3].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 10, 00, 00, 00), result[3].SettlementDay);
            Assert.Equal(3, result[4].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 10, 00, 00, 00), result[4].SettlementDay);
            Assert.Equal(4, result[5].SettlementPeriod);
            Assert.Equal(new DateTime(2018, 04, 10, 00, 00, 00), result[5].SettlementDay);
        }
    }
}
