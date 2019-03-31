using Elexon.FA.BusinessValidation.Domain.Model;
using System.Collections.Generic;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.DISBSADFLOW
{
    public class DisbsadMockData
    {
        public List<Disbsad> GetDibsads()
        {
            List<Disbsad> disbsads = new List<Disbsad>
            {
                new Disbsad
                {
                    DataType = "DISBSAD",
                    SettDate = "2015-12-22",
                    SettlementPeriod = 2,
                    Cost = decimal.MinValue,
                    Id = 1,
                    Soflag = "true",
                    StorFlag = "false",
                    Volume = decimal.MinValue
                },
                new Disbsad
                {
                    DataType = "DISBSAD",
                    SettDate = "2015-12-22",
                    SettlementPeriod = 2,
                    Cost = decimal.MinValue,
                    Id = 1,
                    Soflag = "true",
                    StorFlag = "XDF",
                    Volume = decimal.MinValue
                }

            };
            return disbsads;
        }
    }
}
