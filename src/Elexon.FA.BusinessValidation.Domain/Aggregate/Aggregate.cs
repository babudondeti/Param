namespace Elexon.FA.BusinessValidation.Domain.Aggregate
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.Core.IntegrationMessage;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Aggregate{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Aggregate<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregate{T}"/> class.
        /// </summary>
        /// <param name="item">The item<see cref="Item"/></param>
        /// <param name="businessvalidationflow">The businessvalidationflow<see cref="List{T}"/></param>
        /// <param name="participantEnergyAsset">The participantEnergyAsset<see cref="List{ParticipantEnergyAsset}"/></param>
        /// <param name="updateorINSFlow">The updateorINSFlow<see cref="List{BoalfIndexTable}"/></param>
        public Aggregate(Item item, List<T> businessvalidationflow, List<ParticipantEnergyAsset> participantEnergyAsset, List<BoalfIndexTable> updateorINSFlow)
        {
            Item = item;
            BusinessValidationFlow = businessvalidationflow;
            ParticipantEnergyAsset = participantEnergyAsset;
            UpdateorInsFlow = updateorINSFlow;
            InValidFlow = new List<T>();
            ValidFlow = new List<T>();
            ValidPath = new List<string>();
            InValidPath = new List<string>();
            SettlementPeriodsForFile = new List<int>();
        }

        /// <summary>
        /// Gets or sets the BusinessValidationFlow
        /// </summary>
        public List<T> BusinessValidationFlow { get; }


        /// <summary>
        /// Gets or sets the ParticipantEnergyAsset
        /// </summary>
        public List<ParticipantEnergyAsset> ParticipantEnergyAsset { get; }

        /// <summary>
        /// Gets or sets the SettlementDuration
        /// </summary>
        public int SettlementDuration { get; set; }

        /// <summary>
        /// Gets or sets the LongDay
        /// </summary>
        public DateTime LongDay { get; set; }

        /// <summary>
        /// Gets or sets the ShortDay
        /// </summary>
        public DateTime ShortDay { get; set; }

        /// <summary>
        /// Gets or sets the MinPairId
        /// </summary>
        public int MinPairId { get; set; }

        /// <summary>
        /// Gets or sets the MaxPairId
        /// </summary>
        public int MaxPairId { get; set; }

        /// <summary>
        /// Gets or sets the Item
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Gets or sets the ValidFlow
        /// </summary>
        public List<T> ValidFlow { get; }

        /// <summary>
        /// Gets or sets the InValidFlow
        /// </summary>
        public List<T> InValidFlow { get; }

        /// <summary>
        /// Gets or sets the ValidPath
        /// </summary>
        public List<string> ValidPath { get; }

        /// <summary>
        /// Gets or sets the ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the InValidPath
        /// </summary>
        public List<string> InValidPath { get; }

        /// <summary>
        /// Gets or sets the UpdateorINSFlow
        /// </summary>
        public List<BoalfIndexTable> UpdateorInsFlow { get; }

        /// <summary>
        /// Gets or sets the SettlementPeriodsForFile
        /// </summary>
        public List<int> SettlementPeriodsForFile { get; }

        /// <summary>
        /// Gets or sets a value indicating whether FileAlreadyExistOrNot
        /// </summary>
        public bool FileAlreadyExistOrNot { get; set; }
    }
}
