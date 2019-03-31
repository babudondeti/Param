namespace Elexon.FA.BusinessValidation.Domain.Model
{
    /// <summary>
    /// Defines the <see cref="CreationTime" />
    /// </summary>
    public class CreationTime
    {
        /// <summary>
        /// Gets or sets the Year
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets the Month
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets the Day
        /// </summary>
        public string Day { get; set; }

        /// <summary>
        /// Gets or sets the Hour
        /// </summary>
        public string Hour { get; set; }

        /// <summary>
        /// Gets or sets the Minute
        /// </summary>
        public string Minute { get; set; }

        /// <summary>
        /// Gets or sets the Second
        /// </summary>
        public string Second { get; set; }
    }
}
