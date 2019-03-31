namespace Elexon.FA.BusinessValidation.BOALFlow.Model
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="BoalfModel" />
    /// </summary>
    public class BoalfModel : Boalf
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoalfModel"/> class.
        /// </summary>
        /// <param name="settlementDuration">The settlementDuration<see cref="int"/></param>
        public BoalfModel(int settlementDuration)
        {
            SettlementDuration = settlementDuration;
        }

        /// <summary>
        /// Gets the Settlements
        /// </summary>
        public List<Settlement> Settlements
        {
            get
            {
                return TimeTo.GetMultipleSpForSpanningRecord(TimeFrom, SettlementDuration);
            }
        }

        /// <summary>
        /// Gets or sets the SettlementDuration
        /// </summary>
        public int SettlementDuration { get; set; }
    }
}
