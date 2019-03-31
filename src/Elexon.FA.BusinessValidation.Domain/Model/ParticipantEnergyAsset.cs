namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using System;

    /// <summary>
    /// Defines the <see cref="ParticipantEnergyAsset" />
    /// </summary>
    public class ParticipantEnergyAsset
    {
        /// <summary>
        /// Gets or sets the Effective_From
        /// </summary>
        public DateTime Effective_From { get; set; }

        /// <summary>
        /// Gets or sets the Effective_To
        /// </summary>
        public DateTime Effective_To { get; set; }

        /// <summary>
        /// Gets or sets the Participant_Name
        /// </summary>
        public string Participant_Name { get; set; }
    }
}
