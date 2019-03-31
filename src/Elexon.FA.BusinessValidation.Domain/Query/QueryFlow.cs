namespace Elexon.FA.BusinessValidation.Domain.Query
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.Blob.Abstractions;
    using Elexon.FA.Core.HttpClient;
    using Elexon.FA.Core.KeyVault.Abstractions;
    using Elexon.FA.Core.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="QueryFlow{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryFlow<T> : IQueryFlow<T>
    {
        /// <summary>
        /// Defines the _corehttpServiceClient
        /// </summary>
        private readonly ICoreHttpServiceClient corehttpServiceClient;

        /// <summary>
        /// Defines the _coreKeyVaultClient
        /// </summary>
        private readonly ICoreKeyVaultClient coreKeyVaultClient;

        /// <summary>
        /// Defines the _coreBlobStorage
        /// </summary>
        private readonly ICoreBlobStorage coreBlobStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFlow{T}"/> class.
        /// </summary>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public QueryFlow(IApplicationBuilder applicationBuilder)
        {
            coreBlobStorage = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreBlobStorage>();
            coreKeyVaultClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
            corehttpServiceClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreHttpServiceClient>();
        }

        /// <summary>
        /// The GetListAsync
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{T}}"/></returns>
        public async Task<List<T>> GetListAsync(string filePath, string blobName)
        {
            dynamic jsonData = null;
            try
            {
                var memoryStream = await coreBlobStorage.DownloadAsync(filePath);
                jsonData = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            }
            catch (Exception ex)
            {
                if (BusinessValidationConstants.FLOWS_BOALF != blobName)
                {
                    Log.Error(ex.Message);
                }
            }
            return GetList(jsonData, blobName);
        }

        /// <summary>
        /// The GetBMUParticipationAsync
        /// </summary>
        /// <param name="TimeFrom">The TimeFrom<see cref="DateTime"/></param>
        /// <param name="TimeTo">The TimeTo<see cref="DateTime"/></param>
        /// <returns>The <see cref="Task{List{ParticipantEnergyAsset}}"/></returns>
        public async Task<List<ParticipantEnergyAsset>> GetBmuParticipationAsync(DateTime TimeFrom, DateTime TimeTo)
        {
            List<ParticipantEnergyAsset> result = new List<ParticipantEnergyAsset>();
            try
            {
                string timeFrom = TimeFrom.ToString(BusinessValidationConstants.DATETIMEFORMAT);
                string timeTo = TimeTo.ToString(BusinessValidationConstants.DATETIMEFORMAT);
                var url = GetPath(await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_TABLEAPIURL), timeFrom, timeTo);
                var configSystemParamJsonData = await corehttpServiceClient.GetAsync<List<ParticipantEnergyAsset>>(url);
                result = configSystemParamJsonData;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// The GetPath
        /// </summary>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="timeFrom">The timeFrom<see cref="string"/></param>
        /// <param name="timeTo">The timeTo<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetPath(string relativePath, string timeFrom, string timeTo)
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(relativePath) && timeFrom != null && timeTo != null)
            {
                path = relativePath + BusinessValidationConstants.BMUREFERENCETABLEGET + timeFrom + BusinessValidationConstants.EFFECTIVETO + timeTo;
            }
            return path;
        }

        /// <summary>
        /// The GetList
        /// </summary>
        /// <param name="jsonData">The jsonData<see cref="string"/></param>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="List{T}"/></returns>
        private static List<T> GetList(string jsonData, string blobName)
        {
            List<T> flowLists = new List<T>();
            if (!string.IsNullOrEmpty(jsonData))
            {
                JObject jObject = JObject.Parse(jsonData);
                string flowName = blobName + BusinessValidationConstants.SUFFIX;
                var flows = from p in jObject[flowName] select p;
                flowLists = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(flows));
            }
            return flowLists;
        }

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task DeleteAsync(string blobName)
        {
            try
            {
                await coreBlobStorage.DeleteAsync(blobName);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        /// The GetListBoalfIndexTable
        /// </summary>
        /// <param name="bmuName">The bmuName<see cref="string"/></param>
        /// <param name="boAcceptanceNumber">The boAcceptanceNumber<see cref="string"/></param>
        /// <param name="boAcceptanceTime">The boAcceptanceTime<see cref="string"/></param>
        /// <returns>The <see cref="Task{List{BoalfIndexTable}}"/></returns>
        public async Task<List<BoalfIndexTable>> GetListBoalfIndexTable(string bmuName, string boAcceptanceNumber, string boAcceptanceTime)
        {
            List<BoalfIndexTable> result = new List<BoalfIndexTable>();
            try
            {
                var url = GetPathBoalfIndexTable(await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_TABLEAPIURL), bmuName, boAcceptanceNumber, boAcceptanceTime);
                var configSystemParamJsonData = await corehttpServiceClient.GetAsync<List<BoalfIndexTable>>(url);
                result = configSystemParamJsonData;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// The ExistsAsync
        /// </summary>
        /// <param name="blobName">The blobName<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public Task<bool> ExistsAsync(string blobName)
        {
            var fileExists = coreBlobStorage.ExistsAsync(blobName);

            return fileExists;
        }

        /// <summary>
        /// The GetPathBoalfIndexTable
        /// </summary>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="bmuName">The bmuName<see cref="string"/></param>
        /// <param name="boAcceptanceNumber">The boAcceptanceNumber<see cref="string"/></param>
        /// <param name="boAcceptanceTime">The boAcceptanceTime<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetPathBoalfIndexTable(string relativePath, string bmuName, string boAcceptanceNumber, string boAcceptanceTime)
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(relativePath) && bmuName != null && boAcceptanceNumber != null && boAcceptanceTime != null)
            {
                path = relativePath + BusinessValidationConstants.BOALFINDEXTABLEGET + bmuName + BusinessValidationConstants.BOACCEPTANCENUMBER + 
                    boAcceptanceNumber + BusinessValidationConstants.BOACCEPTANCETIME + boAcceptanceTime;
            }
            return path;
        }
    }
}
