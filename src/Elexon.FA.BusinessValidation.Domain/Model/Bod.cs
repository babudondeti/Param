namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using System;

    /// <summary>
    /// Defines the <see cref="Bod" />
    /// </summary>
    public class Bod
    {
        /// <summary>
        /// Gets or sets the Data
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the BMUName
        /// </summary>
        public string BmuName { get; set; }

        /// <summary>
        /// Gets or sets the TimeFrom
        /// </summary>
        public DateTime TimeFrom { get; set; }

        /// <summary>
        /// Gets or sets the TimeTo
        /// </summary>
        public DateTime TimeTo { get; set; }

        /// <summary>
        /// Gets or sets the PairID
        /// </summary>
        public int PairId { get; set; }

        /// <summary>
        /// Gets or sets the LevelFrom
        /// </summary>
        public int LevelFrom { get; set; }

        /// <summary>
        /// Gets or sets the LevelTo
        /// </summary>
        public int LevelTo { get; set; }

        /// <summary>
        /// Gets or sets the Offer
        /// </summary>
        public decimal Offer { get; set; }

        /// <summary>
        /// Gets or sets the Bid
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// Gets the SettlementPeriod
        /// </summary>
        public int SettlementPeriod
        {
            get
            {
                return TimeTo.GetSettlementPeriod(TimeFrom);
            }
        }

        /// <summary>
        /// Gets the SettlementDay
        /// </summary>
        public string SettlementDay
        {
            get
            {
                return TimeTo.Date.ToString("yyyy-MM-dd");
            }
        }
    }
}
