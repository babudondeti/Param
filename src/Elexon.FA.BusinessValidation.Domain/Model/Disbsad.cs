namespace Elexon.FA.BusinessValidation.Domain.Model
{
    /// <summary>
    /// Defines the <see cref="Disbsad" />
    /// </summary>
    public sealed class Disbsad
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
        /// Gets or sets the ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the Volume
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Gets or sets the Soflag
        /// </summary>
        public string Soflag { get; set; }

        /// <summary>
        /// Gets or sets the StorFlag
        /// </summary>
        public string StorFlag { get; set; }
    }
}
