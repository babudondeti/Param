using Elexon.FA.Core.Blob;
using Elexon.FA.Core.Blob.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Elexon.FA.BusinessValidation.Test.BusinessValidation.NETBSADFLOW
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
            var data = "{ \"Meta Data Attributes\": [{ \"Creation_Time\": [{ \"Year\": \"2018\", \"Month\": \"08\", \"Day\": \"03\", \"Hour\": \"08\", \"Minute\": \"31\", \"Second\": \"07\" }] }],\"NETBSADS\": [{ \"DataType\": \"NETBSAD\", \"SettDate\": \"2015-01-16\", \"SettlementPeriod\": 1, \"NetEnergyBuyPriceCostAdjustment\": 0.0, \"NetEnergyBuyPriceVolumeAdjustment\":0.0, \"NetSystemBuyPriceVolumeAdjustment\": 0.0, \"BuyPricePriceAdjust\": 0.0,\"NetEnergySellPriceCostAdjustment\": 0.0, \"NetEnergySellPriceVolumeAdjustment\": 0.0,\"NetSystemSellPriceVolumeAdjustment\": 0.0,\"SellPricePriceAdjust\": 0.0 }]}";
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
