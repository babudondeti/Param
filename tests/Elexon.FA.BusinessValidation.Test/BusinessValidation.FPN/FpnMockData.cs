using Elexon.FA.BusinessValidation.Domain.Model;
using System;
using System.Collections.Generic;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.FPN
{
    public class FpnMockData
    {
        public List<Fpn> GetFpns()
        {
            List<Fpn> Fpns = new List<Fpn>
            {
                new Fpn
                {
                    BmuName = "GTYPE150",
                    TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 1, 10, 00),
                    PnLevelFrom = 20,
                    PnLevelTo = 40
                },
                new Fpn
                {
                    BmuName = "GTYPE150",
                    TimeFrom = new DateTime(2018, 11, 10, 1, 10, 00),
                    TimeTo = new DateTime(2018, 11, 10, 1, 20, 00),
                    PnLevelFrom = 20,
                    PnLevelTo = 40
                },
                new Fpn
                {
                    BmuName = "GTYPE150",
                    TimeFrom = new DateTime(2018, 11, 10, 1, 20, 00),
                    TimeTo = new DateTime(2018, 11, 10, 1, 30, 00),
                    PnLevelFrom = 20,
                    PnLevelTo = 40
                },
                new Fpn
                {
                    BmuName = "GTYPE151",
                    TimeFrom = new DateTime(2018, 11, 10, 1, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 1, 10, 00),
                    PnLevelFrom = 20,
                    PnLevelTo = 40
                },
                new Fpn
                {
                    BmuName = "GTYPE151",
                    TimeFrom = new DateTime(2018, 11, 10, 1, 10, 00),
                    TimeTo = new DateTime(2018, 11, 10, 1, 20, 00),
                    PnLevelFrom = 20,
                    PnLevelTo = 40
                },
                new Fpn
                {
                    BmuName = "GTYPE151",
                    TimeFrom = new DateTime(2018, 11, 10, 1, 20, 00),
                    TimeTo = new DateTime(2018, 11, 10, 1, 30, 00),
                    PnLevelFrom = 20,
                    PnLevelTo = 40
                }

            };
            return Fpns;
        }

        public List<ParticipantEnergyAsset> GetBMUParticipant()
        {
            List<ParticipantEnergyAsset> ParticipantEnergyAssets = new List<ParticipantEnergyAsset>
            {
                new ParticipantEnergyAsset
                {
                    Participant_Name = "GTYPE150",
                    Effective_From =  new DateTime(2018, 11, 10, 1, 00, 00),
                    Effective_To =  new DateTime(2018, 11, 10, 1, 30, 00)
                },
                new ParticipantEnergyAsset
                {
                    Participant_Name = "GTYPE151",
                    Effective_From =  new DateTime(2018, 11, 10, 1, 00, 00),
                    Effective_To =  new DateTime(2018, 11, 10, 1, 30, 00)
                }
            };
            return ParticipantEnergyAssets;
        }
    }
}
