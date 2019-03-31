using Elexon.FA.Core.Blob;
using Elexon.FA.Core.Blob.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.BOALF
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
            var data = "{ \"Meta Data Attributes\": [{ \"Creation_Time\": [{ \"Year\": \"2018\", \"Month\": \"08\", \"Day\": \"03\", \"Hour\": \"08\", \"Minute\": \"31\", \"Second\": \"07\" }] }], \"BOALFS\": [{ \"Data\": \"BOALF\", \"BMUName\": \"GTYPE1501\", \"TimeFrom\": \"11/5/2018 2:00:00 pm\", \"TimeTo\": \"11/5/2018 2:30:00 pm\", \"PairID\":\"-5\", \"LevelFrom\": -535, \"LevelTo\": -535, \"Offer\": 140, \"Bid\": -9999 }, { \"Data\": \"BOD\", \"BMUName\": \"GTYPE1501\", \"TimeFrom\": \"11/5/2018 2:00:00 pm\", \"TimeTo\": \"11/5/2018 2:30:00 pm\", \"PairID\": 5, \"LevelFrom\": 380, \"LevelTo\": 380, \"Offer\": 140, \"Bid\": 25 }] }";
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
