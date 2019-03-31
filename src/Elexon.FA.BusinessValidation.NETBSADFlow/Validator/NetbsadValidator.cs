namespace Elexon.FA.BusinessValidation.NETBSADFlow.Validator
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using FluentValidation;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="NetbsadValidator" />
    /// </summary>
    public class NetbsadValidator : AbstractValidator<Aggregate<Netbsad>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetbsadValidator"/> class.
        /// </summary>
        public NetbsadValidator()
        {
            RuleSet(BusinessValidationConstants.ERRORCHECK, () =>
            {
                RuleFor(netbsadAggregate => netbsadAggregate).Must((aggregates, aggregate) =>
                    IsAnyRecordExists(aggregate)).WithMessage(BusinessValidationConstants.MSG_ERROR_NODATAFOUND);
            });

            RuleSet(BusinessValidationConstants.WARNINGCHECK, () =>
            {
                RuleFor(netbsadAggregate => netbsadAggregate).Must((aggregates, aggregate) =>
                    IsValidSettlementPeriod(aggregate)).WithMessage(BusinessValidationConstants.MSG_ERROR_INVALIDSETTLEMENTPERIOD);
                RuleFor(netbsadAggregate => netbsadAggregate).Must((aggregates, aggregate) =>
                    IsDuplicateSettlementPeriod(aggregate)).WithMessage(BusinessValidationConstants.MSG_WARNING_DUPLICATEDATA);
            });
        }

        /// <summary>
        /// The IsValidSettlementPeriod
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidSettlementPeriod(Aggregate<Netbsad> aggregate)
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
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        private static void LongDate(Aggregate<Netbsad> aggregate)
        {
            var longDate = aggregate.LongDay.ToString(BusinessValidationConstants.CONFIG_DATEFORMAT);
            AddRangeForLongOrShortDay(aggregate, longDate, BusinessValidationConstants.Range_46);
        }

        /// <summary>
        /// The ShortDate
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        private static void ShortDate(Aggregate<Netbsad> aggregate)
        {
            var shortDate = aggregate.ShortDay.ToString(BusinessValidationConstants.CONFIG_DATEFORMAT);
            AddRangeForLongOrShortDay(aggregate, shortDate, BusinessValidationConstants.Range_50);
        }

        /// <summary>
        /// The AddRangeForLongAndShortDay
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <param name="range">The range<see cref="int"/></param>
        private static void AddRangeForLongAndShortDay(Aggregate<Netbsad> aggregate, int range)
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
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool ShortDayCompare(string settDate, Aggregate<Netbsad> aggregate)
        {
            return (!ExtensionMethod.StringCompare(settDate,
                aggregate.ShortDay.ToString(BusinessValidationConstants.CONFIG_DATEFORMAT)));
        }

        /// <summary>
        /// The AddRangeForLongOrShortDay
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <param name="date">The date<see cref="string"/></param>
        /// <param name="range">The range<see cref="int"/></param>
        private static void AddRangeForLongOrShortDay(Aggregate<Netbsad> aggregate, string date, int range)
        {
            aggregate.InValidFlow.AddRange(aggregate.BusinessValidationFlow.Where
                            (g => ExtensionMethod.StringCompare(g.SettDate, date) &&
                            (g.SettlementPeriod <= 0 || g.SettlementPeriod > range)).ToList());
        }

        /// <summary>
        /// The IsAnyRecordExists
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsAnyRecordExists(Aggregate<Netbsad> aggregate)
        {
            bool IsAnyRecordExists = false;

            if (aggregate.BusinessValidationFlow.Any())
            {
                IsAnyRecordExists = true;
            }


            return IsAnyRecordExists;
        }

        /// <summary>
        /// The IsDuplicateSettlementPeriod
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Netbsad}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsDuplicateSettlementPeriod(Aggregate<Netbsad> aggregate)
        {
            bool IsDuplicateSettlementPeriod = false;

            aggregate.BusinessValidationFlow.Clear();
            aggregate.BusinessValidationFlow.AddRange(aggregate.ValidFlow);
            aggregate.ValidFlow.Clear();

            var netbsadGroupsForUnique = aggregate.BusinessValidationFlow.
                GroupBy(g => new { g.SettDate, g.SettlementPeriod }).Where(c => c.Count() == 1);

            foreach (var netbsadGroupForUnique in netbsadGroupsForUnique)
            {
                aggregate.ValidFlow.AddRange(netbsadGroupForUnique.Select(g => g));
            }

            var netbsadGroupsForDuplicates = aggregate.BusinessValidationFlow.
                GroupBy(g => new { g.SettDate, g.SettlementPeriod }).Where(c => c.Count() > 1);

            foreach (var netbsadGroupForDuplicates in netbsadGroupsForDuplicates)
            {
                var lastRecordInNetbsadGroup = netbsadGroupForDuplicates.Select(g => g).Last();
                aggregate.ValidFlow.Add(lastRecordInNetbsadGroup);
            }

            aggregate.InValidFlow.AddRange(aggregate.BusinessValidationFlow.Except(aggregate.ValidFlow));

            if (!aggregate.InValidFlow.Any())
            {
                IsDuplicateSettlementPeriod = true;
            }
            return IsDuplicateSettlementPeriod;
        }
    }
}
