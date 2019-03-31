namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="BoalfIndexTable" />
    /// </summary>
    public class BoalfIndexTable
    {
        /// <summary>
        /// Gets or sets the PartitionKey
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the RowKey
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Gets or sets the BidOfferAcceptanceNumber
        /// </summary>
        public string BidOfferAcceptanceNumber { get; set; }

        /// <summary>
        /// Gets or sets the BMUName
        /// </summary>
        public string BmuName { get; set; }

        /// <summary>
        /// Gets or sets the AcceptanceTime
        /// </summary>
        public DateTime AcceptanceTime { get; set; }

        /// <summary>
        /// Gets or sets the DeemedBidOfferFlag
        /// </summary>
        public string DeemedBidOfferFlag { get; set; }

        /// <summary>
        /// Gets or sets the SoFlag
        /// </summary>
        public string SoFlag { get; set; }

        /// <summary>
        /// Gets or sets the AmendmentFlag
        /// </summary>
        public string AmendmentFlag { get; set; }

        /// <summary>
        /// Gets or sets the StorFlag
        /// </summary>
        public string StorFlag { get; set; }

        /// <summary>
        /// Gets or sets the SettlementPeriods
        /// </summary>
        public string SettlementPeriods { get; set; }
    }
}
