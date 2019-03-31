namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="Settlement" />
    /// </summary>
    public class Settlement
    {
        /// <summary>
        /// Gets or sets the SettlementPeriod
        /// </summary>
        public int SettlementPeriod { get; set; }

        /// <summary>
        /// Gets or sets the SettlementDay
        /// </summary>
        public DateTime SettlementDay { get; set; }
    }
}
