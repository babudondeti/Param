namespace Elexon.FA.BusinessValidation.BODFlow.Validator
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using FluentValidation;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="BodValidator" />
    /// </summary>
    public class BodValidator : AbstractValidator<Aggregate<Bod>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BodValidator"/> class.
        /// </summary>
        public BodValidator()
        {
            RuleSet(BusinessValidationConstants.ERRORCHECK, () =>
            {
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) => IsTimeFromSmallerThanTimeTo(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_TIMEFROMGREATERTHANTIMETO);
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) => IsValidSettlementDay(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_INVALIDSETTLEMENTDAY);
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) => IsValidSettlementPeriod(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_INVALIDSETTLEMENTPERIOD);
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) => IsValidBmuParticipant(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_INVALIDBMU);
            });

            RuleSet(BusinessValidationConstants.WARNINGCHECK, () =>
            {
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) => IsPairNumberValid(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_INVALIDPAIRID);
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) => IsValidSignForPairIdAndLevel(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_PAIRIDANDLEVELDIFFERENTSIGN);
                RuleFor(bodAggregate => bodAggregate).Must((aggregates, aggregate) => IsBodsNoDuplicateRecord(aggregate)).WithMessage(BusinessValidationConstants.MSG_DUPLICATE);
            });
        }

        /// <summary>
        /// The IsTimeFromSmallerThanTimeTo
        /// </summary>
        /// <param name="bods">The bods<see cref="List{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsTimeFromSmallerThanTimeTo(Aggregate<Bod> aggregate)
        {
            return !aggregate.BusinessValidationFlow.Any(a => a.TimeFrom > a.TimeTo);            
        }

        /// <summary>
        /// The IsValidSettlementDay
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidSettlementDay(Aggregate<Bod> aggregate)
        {
            
            var firstRecord = aggregate.BusinessValidationFlow.FirstOrDefault();       
            return firstRecord != null && !aggregate.BusinessValidationFlow.Any(w => w.TimeFrom != firstRecord.TimeFrom && w.TimeTo != firstRecord.TimeTo);
        }

        /// <summary>
        /// The IsValidSettlementPeriod
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidSettlementPeriod(Aggregate<Bod> aggregate)
        {           
            var record = aggregate.BusinessValidationFlow.FirstOrDefault();
            if (record != null)
            {    
                 return !IsValidSettlementDuration(record.TimeFrom, record.TimeTo, aggregate) || !(aggregate.BusinessValidationFlow.Any(w => w.SettlementPeriod != record.SettlementPeriod));
            }
            return false;
        }
        
        /// <summary>
        /// The IsValidBMUParticipant
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidBmuParticipant(Aggregate<Bod> aggregate)
        {
            bool isValidBMUParticipant = true;
            foreach (var bod in aggregate.BusinessValidationFlow)
            {
                var exist = aggregate.ParticipantEnergyAsset.Any(w => w.Participant_Name == bod.BmuName);
                if (!exist)
                {
                    isValidBMUParticipant = false;    
                }
            }
            return isValidBMUParticipant;
        }

        /// <summary>
        /// The IsValidSettlementDuration
        /// </summary>
        /// <param name="timeFrom">The timeFrom<see cref="DateTime"/></param>
        /// <param name="timeTo">The timeTo<see cref="DateTime"/></param>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidSettlementDuration(DateTime timeFrom, DateTime timeTo, Aggregate<Bod> aggregate)
        {
            bool isValidSettlementDuration = false;
            TimeSpan diff = timeTo.Subtract(timeFrom);
            int duration = diff.Hours * 60 + diff.Minutes;
            if (duration == aggregate.SettlementDuration)
            {
                isValidSettlementDuration = true;
            }
            return isValidSettlementDuration;
        }

        /// <summary>
        /// The IsPairNumberValid
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsPairNumberValid(Aggregate<Bod> aggregate)
        {
            bool isValidPairNumber = true;
            foreach (var bod in aggregate.BusinessValidationFlow)
            {
                if (!(bod.PairId >= aggregate.MinPairId && bod.PairId <= aggregate.MaxPairId && bod.PairId != 0))
                {
                    aggregate.InValidFlow.Add(bod);
                    isValidPairNumber = false;
                }
                else
                {
                    aggregate.ValidFlow.Add(bod);
                }
            }
            return isValidPairNumber;
        }

        /// <summary>
        /// The IsValidSignForPairIdAndLevel
        /// </summary>
        /// <param name="aggregate">The aggregate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidSignForPairIdAndLevel(Aggregate<Bod> aggregate)
        {
            var inValidBods = new List<Bod>();
            var validBods = new List<Bod>();
            bool isValidSignForPairIdAndLevel = true;
            foreach (var bod in aggregate.ValidFlow)
            {
                bool pairIdMinimumLimitCondition = (bod.PairId > 0 && bod.LevelFrom > 0 && bod.LevelTo > 0);
                bool pairIdMaximumLimitCondition = (bod.PairId < 0 && bod.LevelFrom < 0 && bod.LevelTo < 0);
                if (!(pairIdMinimumLimitCondition || pairIdMaximumLimitCondition))
                {
                    inValidBods.Add(bod);
                    isValidSignForPairIdAndLevel = false;
                }
                else
                {
                    validBods.Add(bod);
                }
            }
            aggregate.InValidFlow.AddRange(inValidBods);
            aggregate.ValidFlow.Clear();
            aggregate.ValidFlow.AddRange(validBods);
            return isValidSignForPairIdAndLevel;
        }

        /// <summary>
        /// The IsBodsNoDuplicateRecord
        /// </summary>
        /// <param name="bodAggreate">The bodAggreate<see cref="Aggregate{Bod}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsBodsNoDuplicateRecord(Aggregate<Bod> bodAggreate)
        {
            bool noDuplicateRecord = true;
            List<Bod> inValidBods;
            List<Bod> validBods;
            validBods = bodAggreate.ValidFlow.GroupBy(g => new { g.BmuName, g.PairId, g.SettlementPeriod }).Select(g => g.First()).ToList();
            inValidBods = bodAggreate.ValidFlow.Except(validBods).ToList();
            if (inValidBods.Count > 0)
            {
                noDuplicateRecord = false;
            }
            bodAggreate.InValidFlow.AddRange(inValidBods);
            bodAggreate.ValidFlow.Clear();
            bodAggreate.ValidFlow.AddRange(validBods);
            return noDuplicateRecord;
        }
    }
}
