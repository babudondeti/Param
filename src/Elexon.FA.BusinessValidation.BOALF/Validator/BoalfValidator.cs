namespace Elexon.FA.BusinessValidation.BOALFFlow.Validator
{
    using Elexon.FA.BusinessValidation.Domain.Aggregate;
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using FluentValidation;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="BoalfValidator" />
    /// </summary>
    public class BoalfValidator : AbstractValidator<Aggregate<Boalf>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoalfValidator"/> class.
        /// </summary>
        public BoalfValidator()
        {
            RuleSet(BusinessValidationConstants.WARNINGCHECK, () =>
            {
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsAmendmentFlagOriOrInsOrUpdOrDel(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_AMENDMENTFLAGNOTHAVEORI_OR_INS_OR_DEL_OR_UPD);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsDeemedBidOfferFlagOrSoFlagOrStorFlaghaveOnlyTrueOrFalse(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_DEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_STORFLAGSHOULDBETRUEORFALSE);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsTimeToSmallerThanTimeFrom(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_TIMEFROMGREATERTHANTIMETO);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsFromTimeAndToTimehaveOnlyFourHourDifference(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_FROMTIMEANDTOTIMEHAVEONLYFOURHOURDIFFERENCE);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsSameBoAcceptanceTime(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_DIFFERENTACCEPTANCETIME);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsDeemedBidOfferFlagOrSoFlagOrAmmendmentFlagOrStorFlagSame(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_DIFFERENTDEEMEDBIDOFFERFLAG_OR_SOFLAG_OR_AMMENDMENTFLAG_OR_STORFLAG);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsTimeFromOfPreviousRecordNotEqualToTimetoOfNextRecord(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_DIFFERENTTIMETOOFPREVIOUSRECORDANDTIMEFROMOFNEXTRECORD);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsValidBmuParticipant(aggregate))
                .WithMessage(BusinessValidationConstants.MSG_INVALIDBMU);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsAmmendmentFlagOriOrIns(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_AMMENDMENTFLAG_ORI_OR_INS);
                RuleFor(boalfAggregate => boalfAggregate).Must((aggregates, aggregate) => IsAmmendmentFlagUpdOrDel(aggregate))
                .WithMessage(BusinessValidationConstants.BOALF_MSG_AMMENDMENTFLAG_UPD_OR_DEL);
            });
        }

        /// <summary>
        /// The IsAmendmentFlagORIOrINSOrUPDOrDEL
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsAmendmentFlagOriOrInsOrUpdOrDel(Aggregate<Boalf> boalfAggregate)
        {
            bool isValid = false;
            List<Boalf> ValidBoalfs = boalfAggregate.BusinessValidationFlow.Where(g => g.AmendmentFlag.ToUpperInvariant() == 
            BusinessValidationConstants.AMMENDMENTFLAGDEL.ToUpperInvariant()
            || g.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGINS.ToUpperInvariant()
            || g.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGORI.ToUpperInvariant()
            || g.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGUPD.ToUpperInvariant()).ToList();
            List<Boalf> inValidBoalfs = boalfAggregate.BusinessValidationFlow.Except(ValidBoalfs).ToList();
            if (inValidBoalfs.Count <= 0)
            {
                isValid = true;
            }
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            boalfAggregate.ValidFlow.AddRange(ValidBoalfs);
            return isValid;
        }

        /// <summary>
        /// The IsDeemedBidOfferFlagOrSoFlagOrStorFlaghaveOnlyTrueOrFalse
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsDeemedBidOfferFlagOrSoFlagOrStorFlaghaveOnlyTrueOrFalse(Aggregate<Boalf> boalfAggregate)
        {
            bool isValid = false;

            var boalfDeemedBidOfferFlag = boalfAggregate.ValidFlow.Any(g => (g.DeemedBidOfferFlag.ToUpperInvariant() ==
           BusinessValidationConstants.ISTRUE.ToUpperInvariant()
            || g.DeemedBidOfferFlag.ToUpperInvariant() == BusinessValidationConstants.ISFALSE.ToUpperInvariant()));

            var boalfStorFlag = boalfAggregate.ValidFlow.Any(g => (g.StorFlag.ToUpperInvariant() == BusinessValidationConstants.ISTRUE.ToUpperInvariant()
             || g.StorFlag.ToUpperInvariant() == BusinessValidationConstants.ISFALSE.ToUpperInvariant()));

            var boalfSoFlag = boalfAggregate.ValidFlow.Any(g => (g.SoFlag.ToUpperInvariant() == BusinessValidationConstants.ISTRUE.ToUpperInvariant()
             || g.SoFlag.ToUpperInvariant() == BusinessValidationConstants.ISFALSE.ToUpperInvariant()));

            List<Boalf> ValidBoalfs = boalfAggregate.ValidFlow.Where(g => boalfDeemedBidOfferFlag
            && boalfStorFlag
            && boalfSoFlag).ToList();
            List<Boalf> inValidBoalfs = boalfAggregate.ValidFlow.Except(ValidBoalfs).ToList();
            if (inValidBoalfs.Count <= 0)
            {
                isValid = true;
            }
            inValidBoalfs = inValidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(ValidBoalfs);
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            return isValid;
        }

        /// <summary>
        /// The IsTimeToSmallerThanTimeFrom
        /// </summary>
        /// <param name="boalfAggreagte">The boalfAggreagte<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsTimeToSmallerThanTimeFrom(Aggregate<Boalf> boalfAggreagte)
        {
            bool toTimeGreater = false;
            List<Boalf> inValidBoalfs = boalfAggreagte.ValidFlow.Where(g => g.TimeFrom >= g.TimeTo).ToList();
            if (inValidBoalfs.Count <= 0)
            {
                toTimeGreater = true;
            }
            List<Boalf> validBoalfs = boalfAggreagte.ValidFlow.Except(inValidBoalfs).ToList();
            inValidBoalfs = inValidBoalfs.Except(boalfAggreagte.InValidFlow).ToList();
            boalfAggreagte.ValidFlow.Clear();
            boalfAggreagte.ValidFlow.AddRange(validBoalfs);
            boalfAggreagte.InValidFlow.AddRange(inValidBoalfs);
            return toTimeGreater;
        }

        /// <summary>
        /// The IsFromTimeAndToTimehaveOnlyFourHourDifference
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsFromTimeAndToTimehaveOnlyFourHourDifference(Aggregate<Boalf> boalfAggregate)
        {
            bool timeDifferenceIsFourHour = false;
            List<Boalf> invalidBoalfs = new List<Boalf>();
            var boalfGroups = boalfAggregate.ValidFlow.GroupBy(g => new { g.BmuName, g.BidOfferAcceptanceNumber, g.AcceptanceTime });
            foreach (var boalfGroup in boalfGroups)
            {
                var boalfOrderedGroup = boalfGroup.OrderBy(g => g.TimeTo).ToList();
                var timefromOfEarliestRecord = boalfOrderedGroup.Select(g => g.TimeFrom).FirstOrDefault();
                var timeToOfLatesRecord = boalfOrderedGroup.Select(g => g.TimeTo).LastOrDefault();
                if (timeToOfLatesRecord > timefromOfEarliestRecord.AddHours(BusinessValidationConstants.BOALF_FOURHOURDIFFERENCE))
                {
                    invalidBoalfs.AddRange(boalfGroup);
                }
            }

            if (invalidBoalfs.Count <= 0)
            {
                timeDifferenceIsFourHour = true;
            }
            List<Boalf> validBoalfs = boalfAggregate.ValidFlow.Except(invalidBoalfs).ToList();
            invalidBoalfs = invalidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(validBoalfs);
            boalfAggregate.InValidFlow.AddRange(invalidBoalfs);
            return timeDifferenceIsFourHour;
        }

        /// <summary>
        /// The IsSameBoAcceptanceTime
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsSameBoAcceptanceTime(Aggregate<Boalf> boalfAggregate)
        {
            bool boAcceptanceTimeSame = false;
            List<Boalf> inValidBoalfs = new List<Boalf>();
            var boalfGroups = boalfAggregate.ValidFlow.GroupBy(g => new { g.BmuName, g.BidOfferAcceptanceNumber });
            foreach (var boalfGroup in boalfGroups)
            {
                var firstRecordAcceptanceTime = boalfGroup.Select(g => g.AcceptanceTime).FirstOrDefault();
                var isValidboalf = boalfGroup.Any(g => g.AcceptanceTime != firstRecordAcceptanceTime);
                if (isValidboalf)
                {
                    inValidBoalfs.AddRange(boalfGroup);
                }
            }
            if (inValidBoalfs.Count <= 0)
            {
                boAcceptanceTimeSame = true;
            }
            List<Boalf> validBoalfs = boalfAggregate.ValidFlow.Except(inValidBoalfs).ToList();
            inValidBoalfs = inValidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(validBoalfs);
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            return boAcceptanceTimeSame;
        }

        /// <summary>
        /// The IsDeemedBidOfferFlagOrSoFlagOrAmmendmentFlagOrStorFlagSame
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsDeemedBidOfferFlagOrSoFlagOrAmmendmentFlagOrStorFlagSame(Aggregate<Boalf> boalfAggregate)
        {
            bool deemedBidOfferFlagSame = false;
            List<Boalf> inValidBoalfs = new List<Boalf>();
            var boalfGroups = boalfAggregate.ValidFlow.GroupBy(g => new { g.BmuName, g.BidOfferAcceptanceNumber, g.AcceptanceTime });
            foreach (var boalfGroup in boalfGroups)
            {
                var firstRecordDeeemedBidOfferFlag = boalfGroup.Select(g => g.DeemedBidOfferFlag).FirstOrDefault();
                var firstRecordSoFlag = boalfGroup.Select(g => g.SoFlag).FirstOrDefault();
                var sameAmmendmentFlag = boalfGroup.Select(g => g.AmendmentFlag).FirstOrDefault();
                var firstRecordStorFlag = boalfGroup.Select(g => g.StorFlag).FirstOrDefault();
                var isInValidboalf = boalfGroup.Any(g => g.DeemedBidOfferFlag != firstRecordDeeemedBidOfferFlag
                || g.SoFlag != firstRecordSoFlag
                || g.AmendmentFlag != sameAmmendmentFlag
                || g.StorFlag != firstRecordStorFlag);
                if (isInValidboalf)
                {
                    inValidBoalfs.AddRange(boalfGroup);
                }
            }
            if (inValidBoalfs.Count <= 0)
            {
                deemedBidOfferFlagSame = true;
            }
            List<Boalf> validBoalfs = boalfAggregate.ValidFlow.Except(inValidBoalfs).ToList();
            inValidBoalfs = inValidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(validBoalfs);
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            return deemedBidOfferFlagSame;
        }

        /// <summary>
        /// The IsTimeFromOfPreviousRecordNotEqualToTimetoOfNextRecord
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsTimeFromOfPreviousRecordNotEqualToTimetoOfNextRecord(Aggregate<Boalf> boalfAggregate)
        {
            bool timeNotEqual = false;
            var inValidBoalfs = new List<Boalf>();
            var validBoalfs = new List<Boalf>();
            var boalfGroups = boalfAggregate.ValidFlow.GroupBy(g => new { g.BmuName, g.BidOfferAcceptanceNumber, g.AcceptanceTime });
            foreach (var boalfGroup in boalfGroups)
            {
                var boalfAggregateOrderedTimeFrom = boalfGroup.OrderBy(g => g.TimeFrom).ToList();
                var boalfAggregateOrderedTimeTo = boalfGroup.OrderBy(g => g.TimeTo).ToList();
                validBoalfs = (from boalfs in boalfAggregateOrderedTimeFrom
                               where boalfAggregateOrderedTimeTo.Any(g => g.TimeTo == boalfs.TimeFrom)
                               select boalfs).ToList();
                boalfAggregateOrderedTimeFrom = boalfAggregateOrderedTimeFrom.Except(validBoalfs).ToList();
                var matchingRecords = (from boalfs in boalfAggregateOrderedTimeFrom
                                       where validBoalfs.Any(g => g.TimeFrom == boalfs.TimeTo)
                                       select boalfs).ToList();
                validBoalfs.AddRange(matchingRecords);
                if (boalfGroup.Count() > 1)
                {
                    inValidBoalfs.AddRange(boalfAggregateOrderedTimeFrom.Except(validBoalfs).ToList());
                }
            }

            if (inValidBoalfs.Count <= 0)
            {
                timeNotEqual = true;
            }
            validBoalfs = boalfAggregate.ValidFlow.Except(inValidBoalfs).ToList();
            inValidBoalfs = inValidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(validBoalfs);
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            return timeNotEqual;
        }

        /// <summary>
        /// The IsValidBMUParticipant
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsValidBmuParticipant(Aggregate<Boalf> boalfAggregate)
        {
            bool isValidBMUParticipant = false;

            List<Boalf> validBoalfs = (from boalfs in boalfAggregate.ValidFlow
                                       where boalfAggregate.ParticipantEnergyAsset.Any(g => g.Participant_Name == boalfs.BmuName
                                            && (g.Effective_From.Date == boalfs.TimeFrom.Date
                                            && g.Effective_To.Date == boalfs.TimeTo.Date))
                                       select boalfs).ToList();
            List<Boalf> inValidBoalfs = boalfAggregate.ValidFlow.Except(validBoalfs).ToList();
            if (inValidBoalfs.Count <= 0)
            {
                isValidBMUParticipant = true;
            }
            validBoalfs = boalfAggregate.ValidFlow.Except(inValidBoalfs).ToList();
            inValidBoalfs = inValidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(validBoalfs);
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            return isValidBMUParticipant;
        }

        /// <summary>
        /// The IsAmmendmentFlagORIOrINS
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsAmmendmentFlagOriOrIns(Aggregate<Boalf> boalfAggregate)
        {
            bool ammendmentFlagORIOrINS = false;
            List<Boalf> listAmmendmentFlagORIOrINS = boalfAggregate.ValidFlow.Where(g => g.AmendmentFlag.ToUpperInvariant() == 
            BusinessValidationConstants.AMMENDMENTFLAGINS || g.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGORI).ToList();

            List<Boalf> inValidBoalfs = (from boalfs in listAmmendmentFlagORIOrINS
                                         where boalfAggregate.UpdateorInsFlow.Any(g => g.PartitionKey == boalfs.BmuName
                                             && g.BidOfferAcceptanceNumber == boalfs.BidOfferAcceptanceNumber.ToString()
                                             && g.AcceptanceTime == boalfs.AcceptanceTime
                                             && (g.AmendmentFlag == boalfs.AmendmentFlag))
                                         select boalfs).ToList();

            if (inValidBoalfs.Count <= 0)
            {
                ammendmentFlagORIOrINS = true;
            }
            List<Boalf> validBoalfs = boalfAggregate.ValidFlow.Except(inValidBoalfs).ToList();
            inValidBoalfs = inValidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(validBoalfs);
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            return ammendmentFlagORIOrINS;
        }

        /// <summary>
        /// The IsAmmendmentFlagUPDOrDEL
        /// </summary>
        /// <param name="boalfAggregate">The boalfAggregate<see cref="Aggregate{Boalf}"/></param>
        /// <returns>The <see cref="bool"/></returns>
        private static bool IsAmmendmentFlagUpdOrDel(Aggregate<Boalf> boalfAggregate)
        {
            bool ammendmentFlagUPDOrDEL = false;
            List<Boalf> listAmmendmentFlagUPDOrDEL = boalfAggregate.ValidFlow.Where(g => g.AmendmentFlag.ToUpperInvariant() == 
            BusinessValidationConstants.AMMENDMENTFLAGDEL || g.AmendmentFlag.ToUpperInvariant() == BusinessValidationConstants.AMMENDMENTFLAGUPD).ToList();

            List<Boalf> validBoalf = (from boalfs in listAmmendmentFlagUPDOrDEL
                                      where boalfAggregate.UpdateorInsFlow.Any(g => g.PartitionKey == boalfs.BmuName
                                          && g.BidOfferAcceptanceNumber == boalfs.BidOfferAcceptanceNumber.ToString()
                                          && g.AcceptanceTime == boalfs.AcceptanceTime)
                                      select boalfs).ToList();
            List<Boalf> inValidBoalfs = listAmmendmentFlagUPDOrDEL.Except(validBoalf).ToList();
            if (inValidBoalfs.Count <= 0)
            {
                ammendmentFlagUPDOrDEL = true;
            }
            List<Boalf> validBoalfs = boalfAggregate.ValidFlow.Except(inValidBoalfs).ToList();
            inValidBoalfs = inValidBoalfs.Except(boalfAggregate.InValidFlow).ToList();
            boalfAggregate.ValidFlow.Clear();
            boalfAggregate.ValidFlow.AddRange(validBoalfs);
            boalfAggregate.InValidFlow.AddRange(inValidBoalfs);
            return ammendmentFlagUPDOrDEL;
        }
    }
}
