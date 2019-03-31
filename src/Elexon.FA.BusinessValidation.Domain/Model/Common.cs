namespace Elexon.FA.BusinessValidation.Domain.Model
{
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="Common" />
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// The GetStatusOfBusinessValidation
        /// </summary>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="StatusTableEntity"/></returns>
        public static StatusTableEntity GetStatusOfBusinessValidation(string path)
        {
            var statusTableEntity = new StatusTableEntity
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now,
                BusinessValidationDateTime = Convert.ToString(DateTime.Now),
                BusinessValidationFilePath = path,
                BusinessValidationStatus = BusinessValidationConstants.SUCCESS,
                GenericValidationDateTime = null,
                GenericValidationFilePath = null,
                GenericValidationStatus = null,
                RawFileName = null,
                RawfileArrivedDateTime = null,
                Id = Guid.NewGuid().ToString(),
                RawfileStatus = null,
                RejectedDateTime = null,
                RejectedFolderPath = null,
                RejectedStatus = null
            };

            return statusTableEntity;
        }

        /// <summary>
        /// The GetStatusOfRejectedFile
        /// </summary>
        /// <param name="path">The path<see cref="string"/></param>
        /// <param name="status">The status<see cref="string"/></param>
        /// <returns>The <see cref="StatusTableEntity"/></returns>
        public static StatusTableEntity GetStatusOfRejectedFile(string path, string status)
        {
            var statusTableEntity = new StatusTableEntity
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now,
                BusinessValidationDateTime = Convert.ToString(DateTime.Now),
                BusinessValidationFilePath = path,
                BusinessValidationStatus = status,
                GenericValidationDateTime = null,
                GenericValidationFilePath = null,
                GenericValidationStatus = null,
                RawFileName = null,
                RawfileArrivedDateTime = null,
                Id = Guid.NewGuid().ToString(),
                RawfileStatus = null,
                RejectedDateTime = Convert.ToString(DateTime.Now),
                RejectedFolderPath = path,
                RejectedStatus = status
            };

            return statusTableEntity;
        }

        /// <summary>
        /// The GetOutputFolderPath
        /// </summary>
        /// <param name="SettlementDay">The SettlementDay<see cref="string"/></param>
        /// <param name="SettlementPeriod">The SettlementPeriod<see cref="int"/></param>
        /// <param name="InputPath">The InputPath<see cref="string"/></param>
        /// <param name="OutBound">The OutBound<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetOutputFolderPath(string SettlementDay, int SettlementPeriod, string InputPath, string OutBound)
        {
            string settlementDay = SettlementDay;

            string inputPath = InputPath;

            int settlementPeriod = SettlementPeriod;

            string[] split = inputPath.Split('/');

            DateTime settlementDate = DateTime.ParseExact(settlementDay, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            int year = settlementDate.Year;
            int month = settlementDate.Month;
            int day = settlementDate.Day;


            string path = OutBound;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, split[1], year.ToString(), month.ToString(), day.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, settlementPeriod.ToString(), split[BusinessValidationConstants.Six], 
                   split[BusinessValidationConstants.Seven]).Replace("\\", "/",StringComparison.CurrentCultureIgnoreCase);
            return path;
        }

        /// <summary>
        /// The GetOutputFilePath
        /// </summary>
        /// <param name="SettlementDay">The SettlementDay<see cref="string"/></param>
        /// <param name="SettlementPeriod">The SettlementPeriod<see cref="int"/></param>
        /// <param name="InputPath">The InputPath<see cref="string"/></param>
        /// <param name="OutBound">The OutBound<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetOutputFilePath(string SettlementDay, int SettlementPeriod, string InputPath, string OutBound)
        {
            string settlementDay = SettlementDay;

            string inputPath = InputPath;

            int settlementPeriod = SettlementPeriod;

            string[] nestedPaths = inputPath.Split('/');
            string[] newNestedPaths = new string[nestedPaths.Length + 1];

            DateTime settlementDate = DateTime.ParseExact(settlementDay, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            int year = settlementDate.Year;
            int month = settlementDate.Month;
            int day = settlementDate.Day;

            string fileName = nestedPaths[nestedPaths.Length - 1];


            nestedPaths[(nestedPaths.Length - 1) - 1] = day.ToString();
            nestedPaths[(nestedPaths.Length - 1) - BusinessValidationConstants.Two] = month.ToString();
            nestedPaths[(nestedPaths.Length - 1) - BusinessValidationConstants.Three] = year.ToString();
            nestedPaths[0] = OutBound;
            nestedPaths.CopyTo(newNestedPaths, 0);

            newNestedPaths[(newNestedPaths.Length - 1) - 1] = settlementPeriod.ToString();
            newNestedPaths[newNestedPaths.Length - 1] = fileName;

            string path = string.Empty;

            foreach (var nestedpath in newNestedPaths)
            {
                path = Path.Combine(path, nestedpath).Replace("\\", "/",StringComparison.CurrentCultureIgnoreCase);
            }

            return path;
        }

        /// <summary>
        /// The GetRejectedFolderPath
        /// </summary>
        /// <param name="OutBound">The OutBound<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetRejectedFolderPath(string OutBound)
        {
            if (!Directory.Exists(OutBound))
            {
                Directory.CreateDirectory(OutBound);
            }
            return OutBound + "/" + BusinessValidationConstants.FLOWS_BOALF + "/" + "BOALF.json";
        }

        /// <summary>
        /// The GetProcessingFolderPath
        /// </summary>
        /// <param name="outBound">The outBound<see cref="string"/></param>
        /// <param name="dateTime">The dateTime<see cref="DateTime"/></param>
        /// <param name="settlementPeriod">The settlementPeriod<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string GetProcessingFolderPath(string outBound, DateTime dateTime, string settlementPeriod)
        {
            if (!Directory.Exists(outBound))
            {
                Directory.CreateDirectory(outBound);
            }
            return outBound + "/" + BusinessValidationConstants.FLOWS_BOALF + "/" + dateTime.Date.Year.ToString() + "/" + dateTime.Month.ToString() + "/" + 
                dateTime.Day.ToString() + "/" + settlementPeriod + "/BOALF.json";
        }
    }
}
