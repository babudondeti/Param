namespace Elexon.FA.BusinessValidation.DISBSADFlow.Validator
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="DisbsadValidator" />
    /// </summary>
    public class DisbsadValidator : AbstractValidator<Aggregate<Disbsad>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisbsadValidator"/> class.
        /// </summary>
        public DisbsadValidator()
        {
            RuleSet(BusinessValidationConstants.ERRORCHECK, () =>
            {
                RuleFor(disbsadAggregate => disbsadAggregate).Must((aggregates, aggregate) =>
                    IsAnyRecordExists(aggregate)).WithMessage(BusinessValidationConstants.MSG_ERROR_NODATAFOUND);
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) =>
                    IsValidDateFormat(aggregate)).WithMessage(BusinessValidationConstants.MSG_INVALIDSETTLEMENTDAY);
            });

            RuleSet(BusinessValidationConstants.WARNINGCHECK, () =>
            {

                RuleFor(disbsadAggregate => disbsadAggregate).Must((aggregates, aggregate) =>
                    IsValidSettlementPeriod(aggregate)).WithMessage(BusinessValidationConstants.MSG_ERROR_INVALIDSETTLEMENTPERIOD);
                RuleFor(disbsadAggregate => disbsadAggregate).Must((aggregates, aggregate) =>
                    IsDuplicateSettlementRecord(aggregate)).WithMessage(BusinessValidationConstants.MSG_WARNING_DUPLICATEDATA);
                RuleFor(disbsadAggregate => disbsadAggregate).Must((aggregates, aggregate) =>
                    IsValidSofAndStorFlag(aggregate)).WithMessage(BusinessValidationConstants.DISBSAD_WARNING_MSG_INVALIDSOFANDSTORFLAG);
            });
        }

        /// <summary>
        /// The IsValidDateFormat
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool IsValidDateFormat(Aggregate<Disbsad> aggregate)
        {
            bool IsValidDateFormat = false;
            List<Disbsad> disbisads = aggregate.BusinessValidationFlow;

            foreach (var date in disbisads.Select(s => s.SettDate).ToList())
            {
                string[] formats = { BusinessValidationConstants.CONFIG_DATEFORMAT };
                if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture,
                                          DateTimeStyles.None, out DateTime dt))
                {
                    IsValidDateFormat = true;
                }
            }

            return IsValidDateFormat;
        }

        /// <summary>
        /// The IsValidSettlementPeriod
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidSettlementPeriod(Aggregate<Disbsad> aggregate)
        {
            bool isValidSettlementPeriod = false;
            ShortDate(aggregate);
            LongDate(aggregate);
            AddRangeForLongAndShortDay(aggregate, BusinessValidationConstants.Range_48);
            aggregate.ValidFlow.AddRange(aggregate.BusinessValidationFlow.Except(aggregate.InValidFlow));
            if (!aggregate.InValidFlow.Any())
            {
                isValidSettlementPeriod = true;
            }
            return isValidSettlementPeriod;
        }

        /// <summary>
        /// The LongDate
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        private static void LongDate(Aggregate<Disbsad> aggregate)
        {
            var longDate = aggregate.LongDay.ToString(BusinessValidationConstants.CONFIG_DATEFORMAT);
            AddRangeForLongOrShortDay(aggregate, longDate, BusinessValidationConstants.Range_46);
        }

        /// <summary>
        /// The ShortDate
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        private static void ShortDate(Aggregate<Disbsad> aggregate)
        {
            var shortDate = aggregate.ShortDay.ToString(BusinessValidationConstants.CONFIG_DATEFORMAT);
            AddRangeForLongOrShortDay(aggregate, shortDate, BusinessValidationConstants.Range_50);
        }

        /// <summary>
        /// The AddRangeForLongAndShortDay
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <param name="range">The range<see cref="int"/></param>
        private static void AddRangeForLongAndShortDay(Aggregate<Disbsad> aggregate, int range)
        {
            aggregate.InValidFlow.AddRange(aggregate.BusinessValidationFlow.Where
                (g => (!ExtensionMethod.StringCompare(g.SettDate, aggregate.LongDay.ToString(BusinessValidationConstants.CONFIG_DATEFORMAT)))
                && ShortDayCompare(g.SettDate, aggregate)
                && (g.SettlementPeriod <= 0 || g.SettlementPeriod > range)).ToList());
        }

        /// <summary>
        /// The ShortDayCompare
        /// </summary>
        /// <param name="settDate">The settDate<see cref="string"/></param>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool ShortDayCompare(string settDate, Aggregate<Disbsad> aggregate)
        {
            return (!ExtensionMethod.StringCompare(settDate,
                aggregate.ShortDay.ToString(BusinessValidationConstants.CONFIG_DATEFORMAT)));
        }

        /// <summary>
        /// The AddRangeForLongOrShortDay
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <param name="date">The date<see cref="string"/></param>
        /// <param name="range">The range<see cref="int"/></param>
        private static void AddRangeForLongOrShortDay(Aggregate<Disbsad> aggregate, string date, int range)
        {
            aggregate.InValidFlow.AddRange(aggregate.BusinessValidationFlow.Where
                            (g => ExtensionMethod.StringCompare(g.SettDate, date) &&
                            (g.SettlementPeriod <= 0 || g.SettlementPeriod > range)).ToList());
        }

        /// <summary>
        /// The IsAnyRecordExists
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsAnyRecordExists(Aggregate<Disbsad> aggregate)
        {
            bool IsAnyRecordExists = false;

            if (aggregate.BusinessValidationFlow.Any())
            {
                IsAnyRecordExists = true;
            }
            return IsAnyRecordExists;
        }

        /// <summary>
        /// The IsDuplicateSettlementRecord
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsDuplicateSettlementRecord(Aggregate<Disbsad> aggregate)
        {
            bool IsDuplicateSettlementRecord = true;

            aggregate.BusinessValidationFlow.Clear();
            aggregate.BusinessValidationFlow.AddRange(aggregate.ValidFlow);
            aggregate.ValidFlow.Clear();

            var netbsadGroupsForUnique = aggregate.BusinessValidationFlow.
                GroupBy(g => new { g.SettDate, g.SettlementPeriod, g.Id }).Where(c => c.Count() == 1);

            foreach (var netbsadGroupForUnique in netbsadGroupsForUnique)
            {
                aggregate.ValidFlow.AddRange(netbsadGroupForUnique.Select(g => g));
            }

            var netbsadGroupsForDuplicates = aggregate.BusinessValidationFlow.
                GroupBy(g => new { g.SettDate, g.SettlementPeriod, g.Id }).Where(c => c.Count() > 1);

            foreach (var netbsadGroupForDuplicates in netbsadGroupsForDuplicates)
            {
                var lastRecordInNetbsadGroup = netbsadGroupForDuplicates.Select(g => g).Last();
                aggregate.ValidFlow.Add(lastRecordInNetbsadGroup);
            }

            aggregate.InValidFlow.AddRange(aggregate.BusinessValidationFlow.Except(aggregate.ValidFlow));

            if (aggregate.InValidFlow.Any())
            {
                IsDuplicateSettlementRecord = false;
            }

            return IsDuplicateSettlementRecord;
        }

        /// <summary>
        /// The IsValidSofAndStorFlag
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Disbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidSofAndStorFlag(Aggregate<Disbsad> aggregate)
        {
            bool IsValidSofAndStorFlag = true;

            aggregate.BusinessValidationFlow.Clear();
            aggregate.BusinessValidationFlow.AddRange(aggregate.ValidFlow);
            aggregate.ValidFlow.Clear();
            aggregate.BusinessValidationFlow.ForEach(r =>
            {
                if (bool.TryParse(r.Soflag, out bool convertSofFlagResult) &&
                    bool.TryParse(r.StorFlag, out bool convertStorFlagResult))
                {
                    aggregate.ValidFlow.Add(r);
                }
                else
                {
                    aggregate.InValidFlow.Add(r);
                    IsValidSofAndStorFlag = false;
                }

            });

            return IsValidSofAndStorFlag;
        }
    }
}
