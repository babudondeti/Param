namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="Boalf" />
    /// </summary>
    public class Boalf
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
        /// Gets or sets the BidOfferAcceptanceNumber
        /// </summary>
        public int BidOfferAcceptanceNumber { get; set; }

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
        /// Gets or sets the TimeFrom
        /// </summary>
        public DateTime TimeFrom { get; set; }

        /// <summary>
        /// Gets or sets the TimeTo
        /// </summary>
        public DateTime TimeTo { get; set; }

        /// <summary>
        /// Gets or sets the BidOfferLevelFrom
        /// </summary>
        public int BidOfferLevelFrom { get; set; }

        /// <summary>
        /// Gets or sets the BidOfferLevelTo
        /// </summary>
        public int BidOfferLevelTo { get; set; }

        /// <summary>
        /// Gets or sets the AmendmentFlag
        /// </summary>
        public string AmendmentFlag { get; set; }

        /// <summary>
        /// Gets or sets the StorFlag
        /// </summary>
        public string StorFlag { get; set; }
    }
}
