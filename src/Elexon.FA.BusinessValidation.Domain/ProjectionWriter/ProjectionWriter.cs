namespace Elexon.FA.BusinessValidation.Domain.ProjectionWriter
{
    using Elexon.FA.BusinessValidation.Domain.Model;
    using Elexon.FA.BusinessValidation.Domain.Seed;
    using Elexon.FA.Core.Blob.Abstractions;
    using Elexon.FA.Core.HttpClient;
    using Elexon.FA.Core.KeyVault.Abstractions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ProjectionWriter" />
    /// </summary>
    public class ProjectionWriter : IProjectionWriter
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
        /// Initializes a new instance of the <see cref="ProjectionWriter"/> class.
        /// </summary>
        /// <param name="applicationBuilder">The applicationBuilder<see cref="IApplicationBuilder"/></param>
        public ProjectionWriter(IApplicationBuilder applicationBuilder)
        {
            coreBlobStorage = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreBlobStorage>();
            coreKeyVaultClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreKeyVaultClient>();
            corehttpServiceClient = applicationBuilder?.ApplicationServices.GetRequiredService<ICoreHttpServiceClient>();
        }

        /// <summary>
        /// The UpdateFileStatus
        /// </summary>
        /// <param name="statusTableEntity">The statusTableEntity<see cref="StatusTableEntity"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task UpdateFileStatus(StatusTableEntity statusTableEntity)
        {
            string status = JsonConvert.SerializeObject(statusTableEntity);
            var url = GetPath(await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_STATUSTABLEURL), status);
            await corehttpServiceClient.GetAsync<StatusTableEntity>(url);
        }

        /// <summary>
        /// The InsertBoalfIndex
        /// </summary>
        /// <param name="boalfIndexTable">The boalfIndexTable<see cref="object"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task InsertBoalfIndex(object boalfIndexTable)
        {
            var finalType = new
            {
                BOALFS = boalfIndexTable
            };
            var jsonString = JsonConvert.SerializeObject(finalType, Formatting.None, new IsoDateTimeConverter
            {
                DateTimeFormat = BusinessValidationConstants.DATETIMEFORMAT
            });
            var url = GetPathBoalfIndexInsert(await coreKeyVaultClient.GetSecret(BusinessValidationConstants.KEYVAULT_TABLEAPIURL), jsonString);
            await corehttpServiceClient.GetAsync<BoalfIndexTable>(url);
        }

        /// <summary>
        /// The UpLoadFile
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task UpLoadFile(object value, string path)
        {
            MemoryStream stream = null;

            var jsonString = JsonConvert.SerializeObject(value, Formatting.None, new IsoDateTimeConverter
            {
                DateTimeFormat = BusinessValidationConstants.DATETIMEFORMAT
            });
            
            try
            {
                stream= new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                await coreBlobStorage.UploadAsync(path, stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// The UpLoadFileBoalf
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="path">The path<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task UpLoadFileBoalf(object value, string path)
        {
            MemoryStream stream = null;

            var finalType = new
            {
                Creation_Time = new CreationTime
                {
                    Year = DateTime.Now.ToString("yyyy"),
                    Month = DateTime.Now.ToString("MM"),
                    Day = DateTime.Now.ToString("dd"),
                    Hour = DateTime.Now.ToString("HH"),
                    Minute = DateTime.Now.ToString("mm"),
                    Second = DateTime.Now.ToString("ss")
                },
                BOALFS = value
            };
            var jsonString = JsonConvert.SerializeObject(finalType, Formatting.None, new IsoDateTimeConverter
            {
                DateTimeFormat = BusinessValidationConstants.DATETIMEFORMAT
            });            

            try
            {
                stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                await coreBlobStorage.UploadAsync(path, stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        public async Task DeleteBlob(string blobName)
        {
            await coreBlobStorage.DeleteAsync(blobName);
        }

        /// <summary>
        /// The GetPath
        /// </summary>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="status">The status<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetPath(string relativePath, string status)
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(relativePath) && status != null)
            {

                path = relativePath + BusinessValidationConstants.STATUSTABLE + status;

            }
            return path;
        }

        /// <summary>
        /// The GetPathBoalfIndexInsert
        /// </summary>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="status">The status<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private static string GetPathBoalfIndexInsert(string relativePath, string status)
        {
            string path = string.Empty;
            if (!string.IsNullOrEmpty(relativePath) && status != null)
            {

                path = relativePath + BusinessValidationConstants.BOALFINDEXTABLEINSERT + status;

            }
            return path;
        }
    }
}
