using Elexon.FA.BusinessValidation.Domain.Model;
using System;
using System.Collections.Generic;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOD
{
    public class BodMockData
    {
        public List<Bod>GetBods()
        {
            List<Bod>bods = new List<Bod>
            {
                new Bod
                {
                    Bid = 25,
                    PairId = -5,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 140,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = -380,
                    LevelTo = -380
                },
                new Bod
                {
                    Bid = 24,
                    PairId = -4,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 120,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = -340,
                    LevelTo = -340
                },
                new Bod
                {
                    Bid = 23,
                    PairId = -3,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 100,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = -320,
                    LevelTo = -320
                },
                new Bod
                {
                    Bid = 22,
                    PairId = -2,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 90,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = -300,
                    LevelTo = -300
                },
                new Bod
                {
                    Bid = 21,
                    PairId = -1,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 80,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = -280,
                    LevelTo = -280
                },
                new Bod
                {
                    Bid = 20,
                    PairId = 1,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 70,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = 280,
                    LevelTo = 280
                },
                new Bod
                {
                    Bid = 19,
                    PairId = 2,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 80,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = 260,
                    LevelTo = 260
                },
                new Bod
                {
                    Bid = 18,
                    PairId = 3,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 70,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = 240,
                    LevelTo = 240
                },
                new Bod
                {
                    Bid = 17,
                    PairId = 4,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 60,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = 220,
                    LevelTo = 220
                },
                new Bod
                {
                    Bid = 16,
                    PairId = 5,
                    BmuName = "GTYPE150",
                    Data = "BOD",
                    Offer= 50,
                    TimeFrom = new DateTime(2018, 11, 10, 0, 00, 00),
                    TimeTo = new DateTime(2018, 11, 10, 0, 30, 00),
                    LevelFrom = 200,
                    LevelTo = 200
                }
            };
            return bods;
        }

        public List<ParticipantEnergyAsset> GetBMUParticipant()
        {
            List<ParticipantEnergyAsset> ParticipantEnergyAssets = new List<ParticipantEnergyAsset>
            {
                new ParticipantEnergyAsset
                {
                   
                    Participant_Name = "GTYPE150",
                    Effective_From =  new DateTime(2018, 11, 10, 0, 00, 00),
                    Effective_To =  new DateTime(2018, 11, 10, 0, 30, 00)
                },
                new ParticipantEnergyAsset
                {                  
                   
                    Participant_Name = "GTYPE151",
                    Effective_From =  new DateTime(2018, 11, 10, 0, 00, 00),
                    Effective_To =  new DateTime(2018, 11, 10, 0, 30, 00)
                }
            };
            return ParticipantEnergyAssets;
        }
    }
}
