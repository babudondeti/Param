namespace Elexon.FA.BusinessValidation.Api.Model
{
    /// <summary>
    /// Defines the <see cref="BusinessValidationHealthCheck" />
    /// </summary>
    public class BusinessValidationHealthCheck
    {

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status { get; set; }
        public BusinessValidationHealthCheck()
        {
            Status = "UnHealthy"; 
        }
    }
}
