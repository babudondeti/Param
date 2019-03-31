namespace Elexon.FA.BusinessValidation.FPNFlow.Validator
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using FluentValidation;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="FpnValidator" />
    /// </summary>
    public class FpnValidator : AbstractValidator<Aggregate<Fpn>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FpnValidator"/> class.
        /// </summary>
        public FpnValidator()
        {
            RuleSet(BusinessValidationConstants.ERRORCHECK, () =>
            {
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsAnyRecordExists(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_ERROR_NODATAFOUND);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsFileContainsOnlyOneSettlementDay(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_FILECONTAINSMULTIPLE_SETTLEMENTDAY);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsFileContainsOnlyOneSettlementPeriod(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_FILECONTAINSMULTIPLE_SETTLEMENTPERIOD);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsFileAlreadyExistsOrNot(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_FILEALREADYEXISTSORNOT);
            });

            RuleSet(BusinessValidationConstants.WARNINGCHECK, () =>
            {
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsValidBmuParticipant(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_INVALIDBMU);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsToTimeGreaterThanFromTime(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_TOTIMEGREATERTHANFROMTIME);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsTimeFromOfPreviousRecordNotEqualToTimetoOfNextRecord(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_TIMEFROMPREVIOUSRECORDNOTEQUALTOTIMETONEXTRECORD);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsTimeFromOfAndTimetoOfOneRecordOverlapsWithNextRecord(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_TIMEFROMANDTIMETOOFRECORDOVERLAPSWITHNEXTRECORD);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsStartTimeThereForSameBmUnitRecord(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_STARTTIMETHEREFORSAMEBMUNITRECORD);
                RuleFor(fpnAggregate => fpnAggregate).Must((aggregates, aggregate) => IsEndTimeThereForSameBmUnitRecord(aggregate))
                            .WithMessage(BusinessValidationConstants.MSG_SENDTIMETHEREFORSAMEBMUNITRECORD);
            });
        }

        /// <summary>
        /// The IsFileContainsOnlyOneSettlementDay
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsFileContainsOnlyOneSettlementDay(Aggregate<Fpn> aggregate)
        {
            bool isValidSettlementDay = false;
            var firstRecord = aggregate.BusinessValidationFlow.FirstOrDefault();
            if (firstRecord != null)
            {
                isValidSettlementDay = !(aggregate.BusinessValidationFlow.Any(w => w.SettlementDay != firstRecord.SettlementDay));
            }
            return isValidSettlementDay;
        }

        /// <summary>
        /// The IsFileContainsOnlyOneSettlementPeriod
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsFileContainsOnlyOneSettlementPeriod(Aggregate<Fpn> aggregate)
        {
            bool isValidSettlementPeriod = false;
            var firstRecord = aggregate.BusinessValidationFlow.FirstOrDefault();
            if (firstRecord != null)
            {
                isValidSettlementPeriod = !(aggregate.BusinessValidationFlow.Any(w => w.SettlementPeriod != firstRecord.SettlementPeriod));
            }
            return isValidSettlementPeriod;
        }

        /// <summary>
        /// The IsFileAlreadyExistsOrNot
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsFileAlreadyExistsOrNot(Aggregate<Fpn> aggregate)
        {
            bool IsFileAlreadyExistsorNot = false;
            if (!aggregate.FileAlreadyExistOrNot)
            {
                IsFileAlreadyExistsorNot = true;
            }

            return IsFileAlreadyExistsorNot;
        }

        /// <summary>
        /// The IsAnyRecordExists
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsAnyRecordExists(Aggregate<Fpn> aggregate)
        {
            bool IsAnyRecordExists = false;

            if (aggregate.BusinessValidationFlow.Any())
            {
                IsAnyRecordExists = true;
            }

            return IsAnyRecordExists;
        }

        /// <summary>
        /// The IsValidBMUParticipant
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidBmuParticipant(Aggregate<Fpn> aggregate)
        {
            bool isValidBmuParticipant = false;

            List<Fpn> validFpns =
                    (from fpns in aggregate.BusinessValidationFlow
                     where aggregate.ParticipantEnergyAsset.Any(g => g.Participant_Name == fpns.BmuName && (g.Effective_From.Date == fpns.TimeFrom.Date &&
                     g.Effective_To.Date == fpns.TimeTo.Date))
                     select fpns).ToList();
            List<Fpn> inValidFpns = aggregate.BusinessValidationFlow.Except(validFpns).ToList();
            if (inValidFpns.Count <= 0)
            {
                isValidBmuParticipant = true;
            }
            inValidFpns = inValidFpns.Except(aggregate.InValidFlow).ToList();
            aggregate.ValidFlow.Clear();
            aggregate.ValidFlow.AddRange(validFpns);
            aggregate.InValidFlow.AddRange(inValidFpns);
            return isValidBmuParticipant;
        }

        /// <summary>
        /// The IsToTimeGreaterThanFromTime
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsToTimeGreaterThanFromTime(Aggregate<Fpn> aggregate)
        {
            bool toTimeSmaller = false;
            List<Fpn> inValidFpns;
            List<Fpn> validFpns;
            inValidFpns = aggregate.ValidFlow.Where(g => g.TimeFrom > g.TimeTo).ToList();
            if (inValidFpns.Count == 0)
            {
                toTimeSmaller = true;
            }

            aggregate.InValidFlow.AddRange(inValidFpns);
            validFpns = aggregate.ValidFlow.Except(inValidFpns).ToList();
            aggregate.ValidFlow.Clear();
            aggregate.ValidFlow.AddRange(validFpns);
            return toTimeSmaller;
        }

        /// <summary>
        /// The IsStartTimeThereForSameBmUnitRecord
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsStartTimeThereForSameBmUnitRecord(Aggregate<Fpn> aggregate)
        {
            bool startTimeAvailable = false;
            List<Fpn> inValidFpns;
            List<Fpn> validFpns;
            var fpnGroups = aggregate.ValidFlow.GroupBy(g => new { g.BmuName });
            int recordCount = 0;
            int bmuCount = 0;

            foreach (var fpnGroup in fpnGroups)
            {
                recordCount = fpnGroup.Where(g => g.TimeFrom.TimeOfDay.ToString() == ExtensionMethod.SettlementPeriodStartTime(g.SettlementPeriod)).GroupBy(c => c.BmuName).ToList().Count;
                bmuCount = fpnGroup.GroupBy(c => c.BmuName).ToList().Count;
                if (recordCount == bmuCount)
                {
                    startTimeAvailable = true;
                    validFpns = fpnGroup.OrderBy(g => g.TimeFrom).ToList();
                    AddValidFpns(aggregate, validFpns);
                }
                else
                {
                    startTimeAvailable = false;
                    inValidFpns = fpnGroup.OrderBy(g => g.TimeFrom).ToList();
                    foreach (var inValidFpn in inValidFpns)
                    {
                        aggregate.ValidFlow.Remove(inValidFpn);
                    }

                    aggregate.InValidFlow.AddRange(inValidFpns);
                }
            }
            return startTimeAvailable;
        }

        private static void AddValidFpns(Aggregate<Fpn> aggregate, List<Fpn> validFpns)
        {
            foreach (var validFpn in validFpns)
            {
                if (!aggregate.ValidFlow.Contains(validFpn))
                {
                    aggregate.ValidFlow.Add(validFpn);
                }
            }
        }

        /// <summary>
        /// The IsEndTimeThereForSameBmUnitRecord
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsEndTimeThereForSameBmUnitRecord(Aggregate<Fpn> aggregate)
        {
            bool endTimeAvailable = false;

            List<Fpn> inValidFpns;
            List<Fpn> validFpns;
            var fpnGroups = aggregate.ValidFlow.GroupBy(g => new { g.BmuName });
            int recordCount = 0;
            int bmuCount = 0;

            foreach (var fpnGroup in fpnGroups)
            {
                recordCount = fpnGroup.Where(g => g.TimeTo.TimeOfDay.ToString() == ExtensionMethod.SettlementPeriodEndTime(g.SettlementPeriod)).GroupBy(c => c.BmuName).ToList().Count;
                bmuCount = fpnGroup.GroupBy(c => c.BmuName).ToList().Count;
                if (recordCount == bmuCount)
                {
                    endTimeAvailable = true;
                    validFpns = fpnGroup.OrderBy(g => g.TimeTo).ToList();
                    foreach (var validFpn in validFpns)
                    {
                        AddToValidFlow(aggregate, validFpn);
                    }
                }
                else
                {
                    endTimeAvailable = false;
                    inValidFpns = fpnGroup.OrderBy(g => g.TimeTo).ToList();
                    foreach (var inValidFpn in inValidFpns)
                    {
                        aggregate.ValidFlow.Remove(inValidFpn);
                    }

                    aggregate.InValidFlow.AddRange(inValidFpns);
                }
            }

            return endTimeAvailable;
        }

        private static void AddToValidFlow(Aggregate<Fpn> aggregate, Fpn validFpn)
        {
            if (!aggregate.ValidFlow.Contains(validFpn))
            {
                aggregate.ValidFlow.Add(validFpn);
            }
        }

        /// <summary>
        /// The IsTimeFromOfPreviousRecordNotEqualToTimetoOfNextRecord
        /// </summary>
        /// <param name="fpnAggregate">The fpnAggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsTimeFromOfPreviousRecordNotEqualToTimetoOfNextRecord(Aggregate<Fpn> fpnAggregate)
        {
            bool timeNotEqual = false;
            var inValidFpns = new List<Fpn>();
            var validFpns = new List<Fpn>();
            var fpnGroups = fpnAggregate.ValidFlow.GroupBy(g => new { g.BmuName });
            foreach (var fpnDateContinuity in fpnGroups)
            {
                var fpnOrderedByTimeFrom = fpnDateContinuity.OrderBy(g => g.TimeFrom).ToList();
                var fpnOrderedByTimeTo = fpnDateContinuity.OrderBy(g => g.TimeTo).ToList();
                validFpns =
                    (from fpns in fpnOrderedByTimeFrom
                     where fpnOrderedByTimeTo.Any(g => g.TimeTo == fpns.TimeFrom)
                     select fpns).ToList();
                fpnOrderedByTimeFrom = fpnOrderedByTimeFrom.Except(validFpns).ToList();
                var matchingRecords = (from fpns in fpnOrderedByTimeFrom
                                       where validFpns.Any(g => g.TimeFrom == fpns.TimeTo)
                                       select fpns).ToList();
                validFpns.AddRange(matchingRecords);
                if (fpnDateContinuity.Count() > 1)
                {
                    inValidFpns.AddRange(fpnOrderedByTimeFrom.Except(validFpns).ToList());
                }
            }

            AddValidAndInValidFpnsToAggregate(fpnAggregate, ref timeNotEqual, ref inValidFpns);
            return timeNotEqual;
        }

        private static void AddValidAndInValidFpnsToAggregate(Aggregate<Fpn> fpnAggregate, ref bool timeNotEqual, ref List<Fpn> inValidFpns)
        {
            List<Fpn> validFpns;
            if (inValidFpns.Count <= 0)
            {
                timeNotEqual = true;
            }

            validFpns = fpnAggregate.ValidFlow.Except(inValidFpns).ToList();
            inValidFpns = inValidFpns.Except(fpnAggregate.InValidFlow).ToList();
            fpnAggregate.ValidFlow.Clear();
            fpnAggregate.ValidFlow.AddRange(validFpns);
            fpnAggregate.InValidFlow.AddRange(inValidFpns);
            
        }

        /// <summary>
        /// The IsTimeFromOfAndTimetoOfOneRecordOverlapsWithNextRecord
        /// </summary>
        /// <param name="fpnAggregate">The fpnAggregate<see cref="Aggregate{Fpn}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsTimeFromOfAndTimetoOfOneRecordOverlapsWithNextRecord(Aggregate<Fpn> fpnAggregate)
        {
            bool timeNotEqual = false;
            var inValidFpns = new List<Fpn>();
            List<Fpn> validFpns;
            var fpnGroups = fpnAggregate.ValidFlow.GroupBy(g => new { g.BmuName });
            foreach (var fpnGroup in fpnGroups)
            {
                var fpnAggregateOrderedTimeFrom = fpnGroup.OrderBy(g => g.TimeFrom).ToList();
                validFpns = fpnAggregateOrderedTimeFrom.GroupBy(g => new { g.TimeFrom, g.TimeTo }).Select(g => g.First()).ToList();

                if (fpnGroup.Count() > 1)
                {
                    inValidFpns.AddRange(fpnAggregateOrderedTimeFrom.Except(validFpns).ToList());
                }
            }
            AddValidAndInValidFpnsToAggregate(fpnAggregate, ref timeNotEqual, ref inValidFpns);
            return timeNotEqual;
        }
    }
}
