namespace Elexon.FA.BusinessValidation.Domain.Seed
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.Core.Logging;
    using FluentValidation.Results;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ExtensionMethod" />
    /// </summary>
    public static class ExtensionMethod
    {
        /// <summary>
        /// The GetSettlementPeriod
        /// </summary>
        /// <param name="timeTo">The timeTo<see cref="DateTime"/></param>
        /// <param name="timeFrom">The timeFrom<see cref="DateTime"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int GetSettlementPeriod(this DateTime timeTo, DateTime timeFrom)
        {
            var hour = timeTo.Hour;
            var min = timeTo.Minute;
            string time = hour + ":" + min;
            TimeSpan diff = timeTo.Subtract(timeFrom);
            int duration = diff.Hours * 60 + diff.Minutes;
            decimal timeInDec = Convert.ToDecimal(TimeSpan.Parse(time).TotalHours);
            int settlementPeriod = 0;
            if (duration != 0)
            {
                settlementPeriod = Convert.ToInt32((timeInDec * BusinessValidationConstants.SETTLEMENTPERIOD_CALCULATION_RANGE_60) / duration);
            }
            return settlementPeriod;
        }

        /// <summary>
        /// The IsConsecutive
        /// </summary>
        /// <param name="value">The value<see cref="List{Int32}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsConsecutive(this IEnumerable<Int32> value)
        {
            return !value.Select((i, j) => i - j).Distinct().Skip(1).Any();
        }

        /// <summary>
        /// The GetSettlementPeriodForBV
        /// </summary>
        /// <param name="timeTo">The timeTo<see cref="DateTime"/></param>
        /// <param name="timeFrom">The timeFrom<see cref="DateTime"/></param>
        /// <returns>The <see cref="int"/></returns>
        public static int GetSettlementPeriodForBv(this DateTime timeTo, DateTime timeFrom)
        {
            var hour = timeTo.Hour;
            var min = timeTo.Minute;
            string time = hour + ":" + min;
            int duration = BusinessValidationConstants.SETTLEMENTDURATION;
            decimal timeInDec = Convert.ToDecimal(TimeSpan.Parse(time).TotalHours);
            decimal settlementPeriod = 0;
            int calSettlementPeriod = 0;
            if (duration != 0)
            {
                settlementPeriod = (timeInDec * BusinessValidationConstants.SETTLEMENTPERIOD_CALCULATION_RANGE_60) / duration;
                calSettlementPeriod = Convert.ToInt32(Math.Truncate(settlementPeriod));
                decimal val = settlementPeriod - calSettlementPeriod;
                if (val > 0)
                {
                    calSettlementPeriod++;
                }
            }

            return calSettlementPeriod;
        }

        /// <summary>
        /// The GetMultipleSPForSpanningRecord
        /// </summary>
        /// <param name="timeTo">The timeTo<see cref="DateTime"/></param>
        /// <param name="timeFrom">The timeFrom<see cref="DateTime"/></param>
        /// <param name="settlementDuration">The settlementDuration<see cref="int"/></param>
        /// <returns>The <see cref="List{Settlement}"/></returns>
        public static List<Settlement> GetMultipleSpForSpanningRecord(this DateTime timeTo, DateTime timeFrom, int settlementDuration)
        {
            bool isRecordSpanning = true;
            List<Settlement> settlements = new List<Settlement>();

            var tempTimeFrom = timeFrom;
            DateTime tempTimeTo;
            while (isRecordSpanning)
            {
                if (tempTimeFrom < timeTo)
                {
                    var settlement = new Settlement();
                    if (tempTimeFrom.Minute < BusinessValidationConstants.SETTLEMENTPERIOD_CALCULATION_RANGE_30)
                    {
                        tempTimeTo = ConstructDateTime(tempTimeFrom, BusinessValidationConstants.SETTLEMENTPERIOD_CALCULATION_RANGE_30);
                        settlement.SettlementPeriod = CalculateSettlementPeriod(tempTimeTo, settlementDuration);
                        settlement.SettlementDay = tempTimeFrom.Date;
                        tempTimeFrom = tempTimeTo;
                    }
                    else if (tempTimeFrom.Minute >= BusinessValidationConstants.SETTLEMENTPERIOD_CALCULATION_RANGE_30 &&
                         tempTimeFrom.Minute < BusinessValidationConstants.SETTLEMENTPERIOD_CALCULATION_RANGE_60)
                    {
                        tempTimeTo = ConstructDateTime(tempTimeFrom.AddHours(1), 00);
                        settlement.SettlementPeriod = CalculateSettlementPeriod(tempTimeTo, settlementDuration);
                        settlement.SettlementDay = tempTimeFrom.Date;
                        tempTimeFrom = tempTimeTo;
                    }
                    else
                    {
                        isRecordSpanning = false;
                    }

                    settlements.Add(settlement);
                }
                else
                {
                    isRecordSpanning = false;
                }
            }
            return settlements.Distinct().ToList();
        }

        /// <summary>
        /// The StringCompare
        /// </summary>
        /// <param name="settDate">The settDate<see cref="string"/></param>
        /// <param name="date">The date<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool StringCompare(this string settDate, string date)
        {
            return string.Compare(settDate, date, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// The CalculateSettlementPeriod
        /// </summary>
        /// <param name="time">The time<see cref="DateTime"/></param>
        /// <param name="duration">The duration<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        private static int CalculateSettlementPeriod(DateTime time, int duration)
        {
            var timing = time.Hour + ":" + time.Minute;
            decimal timeInDec = Convert.ToDecimal(TimeSpan.Parse(timing).TotalHours);
            int settlementPeriod = 0;
            if (timeInDec != 0)
            {
                settlementPeriod = Convert.ToInt32((timeInDec * BusinessValidationConstants.SETTLEMENTPERIOD_CALCULATION_RANGE_60) / duration);
            }
            if (timeInDec == 0)
            {
                settlementPeriod = BusinessValidationConstants.SettlementPeriods;
            }
            return settlementPeriod;
        }

        /// <summary>
        /// The ConstructDateTime
        /// </summary>
        /// <param name="time">The time<see cref="DateTime"/></param>
        /// <param name="min">The min<see cref="int"/></param>
        /// <returns>The <see cref="DateTime"/></returns>
        private static DateTime ConstructDateTime(DateTime time, int min)
        {
            var hour = time.Hour;
            return new DateTime(time.Year, time.Month, time.Day, hour, min, 00);
        }

        /// <summary>
        /// The LogErrorMessage
        /// </summary>
        /// <param name="result">The result<see cref="ValidationResult"/></param>
        public static void LogErrorMessage(this ValidationResult result)
        {
            if (result != null)
            {
                var error = result.Errors;
                for (int e = 0; e < error.Count; e++)
                {
                    Log.Error(error[e].ErrorMessage);
                }
            }
        }

        /// <summary>
        /// The SettlementPeriodStartTime
        /// </summary>
        /// <param name="settlementPeriod">The settlementPeriod<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string SettlementPeriodStartTime(int settlementPeriod)
        {
            Double time = Convert.ToDouble(decimal.Divide((settlementPeriod * BusinessValidationConstants.SETTLEMENTDURATION), 60));
            var timeSpan = TimeSpan.FromHours(time);
            DateTime startTime = Convert.ToDateTime(timeSpan.Hours.ToString() + ":" + timeSpan.Minutes.ToString());
            return startTime.AddMinutes(-BusinessValidationConstants.SETTLEMENTDURATION).TimeOfDay.ToString();
        }

        /// <summary>
        /// The SettlementPeriodEndTime
        /// </summary>
        /// <param name="settlementPeriod">The settlementPeriod<see cref="int"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string SettlementPeriodEndTime(int settlementPeriod)
        {
            Double time = Convert.ToDouble(decimal.Divide((settlementPeriod * BusinessValidationConstants.SETTLEMENTDURATION), 60));
            var timeSpan = TimeSpan.FromHours(time);
            DateTime startTime = Convert.ToDateTime(timeSpan.Hours.ToString() + ":" + timeSpan.Minutes.ToString());
            return startTime.TimeOfDay.ToString();
        }
    }
}
