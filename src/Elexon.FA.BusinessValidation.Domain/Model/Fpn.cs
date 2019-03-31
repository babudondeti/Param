namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using System;

    /// <summary>
    /// Defines the <see cref="Fpn" />
    /// </summary>
    public class Fpn
    {
        /// <summary>
        /// Gets or sets the DataType
        /// </summary>
        public string DataType { get; set; }

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
        /// Gets or sets the PNLevelFrom
        /// </summary>
        public int PnLevelFrom { get; set; }

        /// <summary>
        /// Gets or sets the PNLevelTo
        /// </summary>
        public int PnLevelTo { get; set; }

        /// <summary>
        /// Gets the SettlementPeriod
        /// </summary>
        public int SettlementPeriod
        {
            get
            {
                return TimeTo.GetSettlementPeriodForBv(TimeFrom);
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
