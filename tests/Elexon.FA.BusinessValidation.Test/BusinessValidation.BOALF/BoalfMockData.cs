using Elexon.FA.BusinessValidation.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF
{
    public class BoalfMockData
    {
        public List<Boalf> GetBoalfs()
        {
            List<Boalf> boalfs = new List<Boalf>
            {
 new Boalf
                {
                   BmuName="TimeFromAndTimeTo",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,14,00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },

                new Boalf
                {
                   BmuName="DeemedP",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018, 04,09,14, 00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"

                },

                new Boalf
                {
                 BmuName="DeemedP",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="FALSE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="ORI",
                 StorFlag="TRUE"
                },
                new Boalf
                {
                   BmuName="DeemedN",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="TRUE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,14,00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"

                },

                new Boalf
                {
                 BmuName="DeemedN",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="FALSE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="ORI",
                 StorFlag="TRUE"
                },
                new Boalf
                {
                   BmuName="SoFlagP",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018, 04,09,14, 00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"

                },

                new Boalf
                {
                 BmuName="SoFlagP",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="FALSE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="ORI",
                 StorFlag="TRUE"
                },
                new Boalf
                {
                   BmuName="SoFlagN",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018, 04,09,14, 00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },

                new Boalf
                {
                 BmuName="SoFlagN",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="TRUE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="ORI",
                 StorFlag="TRUE"
                },
                 new Boalf
                {
                   BmuName="AmmendmentFlagP",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018, 04,09,14, 00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },

                new Boalf
                {
                 BmuName="AmmendmentFlagP",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="FALSE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="ORI",
                 StorFlag="TRUE"
                },
                  new Boalf
                {
                   BmuName="AmmendmentFlagN",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018, 04,09,14, 00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },

                new Boalf
                {
                 BmuName="AmmendmentFlagN",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="FALSE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="UPD",
                 StorFlag="TRUE"
                },
                  new Boalf
                {
                   BmuName="StorFlagP",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018, 04,09,14, 00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },

                new Boalf
                {
                 BmuName="StorFlagP",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="FALSE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="ORI",
                 StorFlag="TRUE"
                },
                  new Boalf
                {
                   BmuName="StorFlagN",
                   BidOfferAcceptanceNumber=1,
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018, 04,09,14, 00, 00),
                   TimeTo=new DateTime(2018,04,09,14,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },

                new Boalf
                {
                 BmuName="StorFlagN",
                 BidOfferAcceptanceNumber=1,
                 AcceptanceTime=new DateTime(2018,04,09,14,50,00),
                 DeemedBidOfferFlag="FALSE",
                 SoFlag="FALSE",
                 TimeFrom=new DateTime(2018,04,09,14,30,00),
                 TimeTo=new DateTime(2018,04,09,14,40,00),
                 BidOfferLevelFrom=80,
                 BidOfferLevelTo=80,
                 AmendmentFlag="ORI",
                 StorFlag="FALSE"
                },
           new Boalf
           {
               BmuName="ContinuedAP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,00,00),
               BidOfferLevelFrom=-40,
               TimeTo=new DateTime(2018,04,09,14,30,00),
               BidOfferLevelTo=-40,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
           new Boalf
           {
               BmuName="ContinuedAP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,30,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,14,58,00),
               BidOfferLevelTo=80,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
           new Boalf
           {
               BmuName="ContinuedAN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,00,00),
               BidOfferLevelFrom=-40,
               TimeTo=new DateTime(2018,04,09,14,30,00),
               BidOfferLevelTo=-40,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },

        new Boalf
           {
               BmuName="ContinuedAN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,12,00,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,12,30,00),
               BidOfferLevelTo=80,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
                   new Boalf
           {
               BmuName="BMUCheckP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,00,00),
               BidOfferLevelFrom=-40,
               TimeTo=new DateTime(2018,04,09,14,30,00),
               BidOfferLevelTo=-40,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },

        new Boalf
           {
               BmuName="BMUCheckP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,30,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,14,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
           new Boalf
           {
               BmuName="BMUCheckN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,30,00),
               BidOfferLevelFrom=-40,
               TimeTo=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelTo=-40,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
        new Boalf
           {
               BmuName="BMUCheckN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,16,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
            new Boalf
           {
               BmuName="ORIP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
           new Boalf
           {
               BmuName="ORIN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="ORI",
               StorFlag="TRUE"
           },
             new Boalf
           {
               BmuName="DELP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="DEL",
               StorFlag="TRUE"
           },
           new Boalf
           {
               BmuName="DELN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="UPD",
               StorFlag="TRUE"
           },
              new Boalf
           {
               BmuName="INSP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="INS",
               StorFlag="TRUE"
           },
           new Boalf
           {
               BmuName="INSN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="INS",
               StorFlag="TRUE"
           },
             new Boalf
           {
               BmuName="UPDP",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="UPD",
               StorFlag="TRUE"
           },
           new Boalf
           {
               BmuName="UPDN",
               BidOfferAcceptanceNumber=1,
               AcceptanceTime=new DateTime(2018,04,09,14,50,00),
               DeemedBidOfferFlag="FALSE",
               SoFlag="FALSE",
               TimeFrom=new DateTime(2018,04,09,14,55,00),
               BidOfferLevelFrom=80,
               TimeTo=new DateTime(2018,04,09,15,50,00),
               BidOfferLevelTo=80,
               AmendmentFlag="UPD",
               StorFlag="TRUE"
           },
            };
            return boalfs;
        }

        public List<ParticipantEnergyAsset> GetBMUParticipant()
        {
            List<ParticipantEnergyAsset> ParticipantEnergyAssets = new List<ParticipantEnergyAsset>
            {
                new ParticipantEnergyAsset
                {
                    Participant_Name = "TimeFromAndTimeTo",
                    Effective_From =new DateTime(2018,04,09,14,00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)

                },
                new ParticipantEnergyAsset
                {
                    Participant_Name = "DeemedP",
                    Effective_From =  new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },

                new ParticipantEnergyAsset
                {
                    Participant_Name = "DeemedN",
                    Effective_From =   new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =   new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {

                    Participant_Name = "SoFlagP",
                    Effective_From =  new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {

                    Participant_Name = "SoFlagN",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {

                    Participant_Name = "AmmendmentFlagP",
                    Effective_From =  new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {

                    Participant_Name = "AmmendmentFlagN",
                    Effective_From =   new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {

                    Participant_Name = "StorFlagP",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {

                    Participant_Name = "StorFlagN",
                    Effective_From =  new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {

                    Participant_Name = "ContinuedAP",
                    Effective_From =  new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {

                    Participant_Name = "ContinuedAN",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {
                    Participant_Name = "BMUCheckP",
                    Effective_From =  new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {

                    Participant_Name = "BMUCheckP",
                    Effective_From =  new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                      new ParticipantEnergyAsset
                {

                    Participant_Name = "BMUCheckNN",
                    Effective_From =   new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To =  new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {

                    Participant_Name = "BMUCheckNN",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {
                    Participant_Name = "ORIP",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {
                    Participant_Name = "ORIN",
                    Effective_From = new DateTime(2018,04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {
                    Participant_Name = "DELP",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {
                    Participant_Name = "DELN",
                    Effective_From = new DateTime(2018,04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {
                    Participant_Name = "INSP",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {
                    Participant_Name = "INSN",
                    Effective_From = new DateTime(2018,04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                 new ParticipantEnergyAsset
                {
                    Participant_Name = "UPDP",
                    Effective_From = new DateTime(2018, 04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },
                new ParticipantEnergyAsset
                {
                    Participant_Name = "UPDN",
                    Effective_From = new DateTime(2018,04,09,14, 00, 00),
                    Effective_To = new DateTime(2018,04,09,14,30, 00)
                },

            };
            return ParticipantEnergyAssets;
        }

        public List<BoalfIndexTable> GetUpdateorINSFlow()
        {
            List<BoalfIndexTable> updateorINSFlow = new List<BoalfIndexTable>
            {
                new BoalfIndexTable
               {
                   PartitionKey="ORIP",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="UPD",
                   StorFlag="TRUE"
                },
                new BoalfIndexTable
                {
                   PartitionKey="ORIN",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },
                 new BoalfIndexTable
               {
                   PartitionKey="DELP",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="DEL",
                   StorFlag="TRUE"
                },
                new BoalfIndexTable
                {
                   PartitionKey="DELN",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },
                 new BoalfIndexTable
               {
                   PartitionKey="INSP",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="UPD",
                   StorFlag="TRUE"
                },
                new BoalfIndexTable
                {
                   PartitionKey="INSN",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="INS",
                   StorFlag="TRUE"
                },
                 new BoalfIndexTable
               {
                   PartitionKey="UPDP",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="1",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="UPD",
                   StorFlag="TRUE"
                },
                new BoalfIndexTable
                {
                   PartitionKey="UPDN",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="11",
                   AcceptanceTime =new DateTime(2018,04,09,14,50, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="ORI",
                   StorFlag="TRUE"
                },

            };
            return updateorINSFlow;
        }

        public List<Boalf> GetFileProcessData()
        {
            List<Boalf> boalfs = new List<Boalf>
            {
              new Boalf{
                   BmuName="DRAXD-1",
                   BidOfferAcceptanceNumber=2240,
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,00,15, 00),
                   TimeTo=new DateTime(2018,04,09,00,30, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="UPD",
                   StorFlag="Ramesh"
               },
              new Boalf{
                   BmuName="DRAXD-1",
                   BidOfferAcceptanceNumber=2240,
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,00,30, 00),
                   TimeTo=new DateTime(2018,04,09,01,00, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="UPD",
                   StorFlag="Ramesh"
               },
              new Boalf
              {
                  BmuName="DRAXD-2",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,00,35,00),
                  TimeTo=new DateTime(2018,04,09,00,40,00),
                  BidOfferLevelFrom=150,
                  BidOfferLevelTo=150,
                  AmendmentFlag="UPD",
                  StorFlag="Benni"
              },
               new Boalf
              {
                  BmuName="DRAXD-2",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,00,40,00),
                  TimeTo=new DateTime(2018,04,09,00,01,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="UPD",
                  StorFlag="Benni"
              },
              new Boalf
              {
                  BmuName="DRAXD-1",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,00,15,00),
                  TimeTo=new DateTime(2018,04,09,00,30,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="ORI",
                  StorFlag="False"
              },
              new Boalf
              {
                  BmuName="DRAXD-2",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,00,30,00),
                  TimeTo=new DateTime(2018,04,09,01,15,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="ORI",
                  StorFlag="False"
              },
              new Boalf
              {
                  BmuName="DRAXD-3",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,01,30,00),
                  TimeTo=new DateTime(2018,04,09,02,30,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="ORI",
                  StorFlag="False"
              },
              new Boalf
              {
                  BmuName="DRAXD-1",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,00,30,00),
                  TimeTo=new DateTime(2018,04,09,00,40,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="UPD",
                  StorFlag="UpdateReceived"
              },
            };
            return boalfs;
        }
        public List<BoalfIndexTable> GetUpdateorINSForFileProcess()
        {
            List<BoalfIndexTable> updateorINSFlow = new List<BoalfIndexTable>
            {
                new BoalfIndexTable
               {
                   PartitionKey="DRAXD-1",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="2240",
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="UPD",
                   StorFlag="TRUE",
                   SettlementPeriods="29,30,31"
                },
                new BoalfIndexTable
                {
                   PartitionKey="DRAXD-1",
                   RowKey=Guid.NewGuid().ToString(),
                   BidOfferAcceptanceNumber="2240",
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   AmendmentFlag="UPD",
                   StorFlag="TRUE",
                   SettlementPeriods="29,30"
                },
                 new BoalfIndexTable
              {
                  PartitionKey="DRAXD-2",
                  RowKey=Guid.NewGuid().ToString(),
                  BidOfferAcceptanceNumber="2240",
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  AmendmentFlag="UPD",
                  StorFlag="TRUE",
                   SettlementPeriods="1,2"
              },
               new BoalfIndexTable
              {
                  PartitionKey="DRAXD-2",
                  BidOfferAcceptanceNumber="2240",
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  AmendmentFlag="UPD",
                  StorFlag="TRUE",
                  SettlementPeriods="1,2"
              },
              new BoalfIndexTable
              {
                  PartitionKey="DRAXD-1",
                  BidOfferAcceptanceNumber="2240",
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  AmendmentFlag="ORI",
                  StorFlag="TRUE",
                  SettlementPeriods="1,2,3,4,5"
              },
              new BoalfIndexTable
              {
                  PartitionKey="DRAXD-2",
                  BidOfferAcceptanceNumber="2240",
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  AmendmentFlag="ORI",
                  StorFlag="TRUE",
                  SettlementPeriods="1,2,3,4,5"
              },
              new BoalfIndexTable
              {
                  PartitionKey="DRAXD-3",
                  BidOfferAcceptanceNumber="2240",
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  AmendmentFlag="ORI",
                  StorFlag="TRUE",
                  SettlementPeriods="1,2,3,4,5"
              },

            };
            return updateorINSFlow;
        }
        public List<Boalf> GetBlobDataForFileProcess()
        {
            List<Boalf> boalfs = new List<Boalf>
            {
              new Boalf{
                   BmuName="DRAXD-1",
                   BidOfferAcceptanceNumber=2240,
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,14,00, 00),
                   TimeTo=new DateTime(2018,04,09,14,35, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="UPD",
                   StorFlag="TRUE"
               },
              new Boalf{
                   BmuName="DRAXD-1",
                   BidOfferAcceptanceNumber=2240,
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,14,35, 00),
                   TimeTo=new DateTime(2018,04,09,15,01, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="UPD",
                   StorFlag="TRUE"
               },
               new Boalf
              {
                  BmuName="DRAXD-2",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,14,20,00),
                  TimeTo=new DateTime(2018,04,09,14,22,00),
                  BidOfferLevelFrom=50,
                  BidOfferLevelTo=50,
                  AmendmentFlag="UPD",
                  StorFlag="False"
              },
               new Boalf
              {
                  BmuName="DRAXD-2",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,14,22,00),
                  TimeTo=new DateTime(2018,04,09,14,35,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="UPD",
                  StorFlag="False"
              },
                new Boalf
              {
                  BmuName="DRAXD-1",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,00,15,00),
                  TimeTo=new DateTime(2018,04,09,00,30,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="ORI",
                  StorFlag="False"
              },
              new Boalf
              {
                  BmuName="DRAXD-2",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,00,30,00),
                  TimeTo=new DateTime(2018,04,09,01,15,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="ORI",
                  StorFlag="False"
              },
              new Boalf
              {
                  BmuName="DRAXD-3",
                  BidOfferAcceptanceNumber=2240,
                  AcceptanceTime=new DateTime(2018,04,09,14,32,00),
                  DeemedBidOfferFlag="FALSE",
                  SoFlag="FALSE",
                  TimeFrom=new DateTime(2018,04,09,01,30,00),
                  TimeTo=new DateTime(2018,04,09,02,30,00),
                  BidOfferLevelFrom=80,
                  BidOfferLevelTo=80,
                  AmendmentFlag="ORI",
                  StorFlag="False"
              },
            };
            return boalfs;
        }
        public List<Boalf> GetFileProcessDataRejectedAcceptance()
        {
            List<Boalf> boalfs = new List<Boalf>
            {
              new Boalf{
                   BmuName="DRAXD-1",
                   BidOfferAcceptanceNumber=2240,
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,14,00, 00),
                   TimeTo=new DateTime(2018,04,09,14,35, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="UPD",
                   StorFlag="TRUE"
               },
              new Boalf{
                   BmuName="DRAXD-1",
                   BidOfferAcceptanceNumber=2241,
                   AcceptanceTime =new DateTime(2018,04,09,14,32, 00),
                   DeemedBidOfferFlag="FALSE",
                   SoFlag="FALSE",
                   TimeFrom=new DateTime(2018,04,09,14,30, 00),
                   TimeTo=new DateTime(2018,04,09,15,01, 00),
                   BidOfferLevelFrom=150,
                   BidOfferLevelTo=150,
                   AmendmentFlag="UPD",
                   StorFlag="TRUE"
               }
            };
            return boalfs;
        }      

        public async Task<BusinessValidationProxy> BusinessValidationProxies()
        {
            BusinessValidationProxy businessValidationProxies = new BusinessValidationProxy();

            businessValidationProxies.ValidPaths.Add("Processing/SAA-I00V-Boalf/2018/10/24/29/Boalf/BOALF.json");
            businessValidationProxies.InValidPaths.Add("Rejection/SAA-I00V-Boalf/2018/10/24/29/Boalf/BOALF.json");
            businessValidationProxies.Valid = true;
            businessValidationProxies.InValid = true;
            return businessValidationProxies;
            
        }


    }
}
