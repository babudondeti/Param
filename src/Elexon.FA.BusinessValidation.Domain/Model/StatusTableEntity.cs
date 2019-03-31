namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using Elexon.FA.Core.TableStorage;
    using System;

    /// <summary>
    /// Defines the <see cref="StatusTableEntity" />
    /// </summary>
    public class StatusTableEntity : CoreTableEntity
    {
        /// <summary>
        /// Gets or sets the BusinessValidationDateTime
        /// </summary>
        public string BusinessValidationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the BusinessValidationFilePath
        /// </summary>
        public string BusinessValidationFilePath { get; set; }

        /// <summary>
        /// Gets or sets the BusinessValidationStatus
        /// </summary>
        public string BusinessValidationStatus { get; set; }

        /// <summary>
        /// Gets or sets the GenericValidationDateTime
        /// </summary>
        public string GenericValidationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the GenericValidationFilePath
        /// </summary>
        public string GenericValidationFilePath { get; set; }

        /// <summary>
        /// Gets or sets the GenericValidationStatus
        /// </summary>
        public string GenericValidationStatus { get; set; }

        /// <summary>
        /// Gets or sets the RawFileName
        /// </summary>
        public string RawFileName { get; set; }

        /// <summary>
        /// Gets or sets the RawfileArrivedDateTime
        /// </summary>
        public string RawfileArrivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public new string Id { get; set; }

        /// <summary>
        /// Gets or sets the RawfileStatus
        /// </summary>
        public String RawfileStatus { get; set; }

        /// <summary>
        /// Gets or sets the RejectedDateTime
        /// </summary>
        public string RejectedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the RejectedFolderPath
        /// </summary>
        public string RejectedFolderPath { get; set; }

        /// <summary>
        /// Gets or sets the RejectedStatus
        /// </summary>
        public string RejectedStatus { get; set; }
    }
}
