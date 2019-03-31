namespace Elexon.FA.BusinessValidation.Domain.Model
{
    /// <summary>
    /// Defines the <see cref="Netbsad" />
    /// </summary>
    public sealed class Netbsad
    {
        /// <summary>
        /// Gets or sets the DataType
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the SettDate
        /// </summary>
        public string SettDate { get; set; }

        /// <summary>
        /// Gets or sets the SettlementPeriod
        /// </summary>
        public int SettlementPeriod { get; set; }

        /// <summary>
        /// Gets or sets the NetEnergyBuyPriceCostAdjustment
        /// </summary>
        public decimal NetEnergyBuyPriceCostAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the NetEnergyBuyPriceVolumeAdjustment
        /// </summary>
        public decimal NetEnergyBuyPriceVolumeAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the NetSystemBuyPriceVolumeAdjustment
        /// </summary>
        public decimal NetSystemBuyPriceVolumeAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the BuyPricePriceAdjust
        /// </summary>
        public decimal BuyPricePriceAdjust { get; set; }

        /// <summary>
        /// Gets or sets the NetEnergySellPriceCostAdjustment
        /// </summary>
        public decimal NetEnergySellPriceCostAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the NetEnergySellPriceVolumeAdjustment
        /// </summary>
        public decimal NetEnergySellPriceVolumeAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the NetSystemSellPriceVolumeAdjustment
        /// </summary>
        public decimal NetSystemSellPriceVolumeAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the SellPricePriceAdjust
        /// </summary>
        public decimal SellPricePriceAdjust { get; set; }

        /// <summary>
        /// The ResetBuySellPriceVolumeAdjustment
        /// </summary>
        public void ResetBuySellPriceVolumeAdjustment()
        {
            NetEnergyBuyPriceCostAdjustment = decimal.Zero;
            NetEnergyBuyPriceVolumeAdjustment = decimal.Zero;
            NetSystemBuyPriceVolumeAdjustment = decimal.Zero;
            NetEnergySellPriceCostAdjustment = decimal.Zero;
            NetEnergySellPriceVolumeAdjustment = decimal.Zero;
            NetSystemSellPriceVolumeAdjustment = decimal.Zero;
        }
    }
}
