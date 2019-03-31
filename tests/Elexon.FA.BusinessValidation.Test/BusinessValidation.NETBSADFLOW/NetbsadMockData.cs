using Elexon.FA.BusinessValidation.Domain.Model;
using System.Collections.Generic;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.NETBSADFLOW
{
    public class NetbsadMockData
    {
        public List<Netbsad> GetNetbsads()
        {
            List<Netbsad> netbsads = new List<Netbsad>
            {
                new Netbsad
                {
                    DataType = "NETBSAD",
                    SettDate = "2015-12-22",
                    SettlementPeriod = 2,
                    BuyPricePriceAdjust = decimal.Zero,
                    NetEnergyBuyPriceCostAdjustment = decimal.Zero,
                    NetEnergyBuyPriceVolumeAdjustment = decimal.Zero,
                    NetEnergySellPriceCostAdjustment = decimal.Zero,
                    NetEnergySellPriceVolumeAdjustment = decimal.Zero,
                    NetSystemBuyPriceVolumeAdjustment = decimal.Zero,
                    NetSystemSellPriceVolumeAdjustment = decimal.Zero,
                    SellPricePriceAdjust = decimal.Zero
                },
                new Netbsad
                {
                    DataType = "NETBSAD",
                    SettDate = "2015-12-22",
                    SettlementPeriod = 2,
                    BuyPricePriceAdjust = decimal.Zero,
                    NetEnergyBuyPriceCostAdjustment = decimal.Zero,
                    NetEnergyBuyPriceVolumeAdjustment = decimal.Zero,
                    NetEnergySellPriceCostAdjustment = decimal.Zero,
                    NetEnergySellPriceVolumeAdjustment = decimal.Zero,
                    NetSystemBuyPriceVolumeAdjustment = decimal.Zero,
                    NetSystemSellPriceVolumeAdjustment = decimal.Zero,
                    SellPricePriceAdjust = decimal.Zero
                }
            };
            return netbsads;
        }
    }
}
