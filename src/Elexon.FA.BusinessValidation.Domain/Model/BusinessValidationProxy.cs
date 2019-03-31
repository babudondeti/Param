namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="BusinessValidationProxy" />
    /// </summary>
    public class BusinessValidationProxy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessValidationProxy"/> class.
        /// </summary>
        public BusinessValidationProxy()
        {
            ValidPaths = new List<string>();
            InValidPaths = new List<string>();
            ErrorMessages = new List<string>();
        }

        /// <summary>
        /// Gets or sets the ValidPaths
        /// </summary>
        public List<string> ValidPaths { get;}

        /// <summary>
        /// Gets or sets the ErrorMessages
        /// </summary>
        public List<string> ErrorMessages { get; }

        /// <summary>
        /// Gets or sets the InValidPaths
        /// </summary>
        public List<string> InValidPaths { get;}

        /// <summary>
        /// Gets or sets a value indicating whether Valid
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether InValid
        /// </summary>
        public bool InValid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ValidationResult
        /// </summary>
        public bool ValidationResult { get; set; }
    }
}
