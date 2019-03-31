using Elexon.FA.Core.Blob;
using Elexon.FA.Core.Blob.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.FPN
{
    public class MockCoreBlobStorage : ICoreBlobStorage
    {
        public MockCoreBlobStorage()
        {
        }

        public Task DeleteAsync(string blobName)
        {
            throw new NotImplementedException();
        }



        public Task<MemoryStream> DownloadAsync(string blobName)
        {
            var data = "{ \"Meta Data Attributes\": [{ \"Creation_Time\": [{ \"Year\": \"2018\", \"Month\": \"08\", \"Day\": \"03\", \"Hour\": \"08\", \"Minute\": \"31\", \"Second\": \"07\" }] }], \"FPNS\": [{\"DataType\": \"PN\",\"BMUName\": \"ABERU-1\",\"TimeFrom\": \"2018-11-10 1:00\",\"PNLevelFrom\": 0,\"TimeTo\": \"2018-11-10 1:15\",\"PNLevelTo\": 0 }, {\"DataType\": \"PN\",\"BMUName\": \"ABERU-1\",\"TimeFrom\": \"2018-11-10 1:15\",\"PNLevelFrom\": 0,\"TimeTo\": \"2018-11-10 1:30\",\"PNLevelTo\": 0 }] }";
            var bytes = Encoding.Default.GetBytes(data);
            var buffer = new MemoryStream(bytes, 0, bytes.Length, true, true);
            return Task.FromResult(buffer);
        }

        public Task DownloadAsync(string blobName, string path)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<List<CoreBlobItem>> ListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CoreBlobItem>> ListAsync(string rootFolder)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ListFoldersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> ListFoldersAsync(string rootFolder)
        {
            throw new NotImplementedException();
        }

        public Task UploadAsync(string blobName, string filePath)
        {
            return Task.CompletedTask;
        }

        public Task UploadAsync(string blobName, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
