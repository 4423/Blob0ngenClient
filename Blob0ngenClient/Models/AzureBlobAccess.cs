using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Blob0ngenClient.Models
{
    public class DownloadProgressChangedEventArgs : EventArgs
    {
        public DownloadProgressChangedEventArgs(long fileSize, long downloadedSize)
        {
            this.FileSize = fileSize;
            this.DownloadedSize = downloadedSize;
        }

        public long FileSize { get; private set; }
        public long DownloadedSize { get; private set; }
        public bool IsCompleted => FileSize == DownloadedSize;
    }

    public class AzureBlobAccess
    {
        private string accountName;
        private string accessKey;

        private CloudBlobClient blobClient;
        
        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

        public AzureBlobAccess()
        {
            var settings = ResourceLoader.GetForCurrentView();
            accessKey = settings.GetString("AzureBlobAccessKey");
            accountName = settings.GetString("AzureBlobAccountName");

            var credential = new StorageCredentials(accountName, accessKey);
            var storageAccount = new CloudStorageAccount(credential, true);
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<byte[]> DownloadBytesAsync(string blobPath)
        {
            var containerName = GetContainerName(blobPath);
            var container = blobClient.GetContainerReference(containerName);

            var blobName = GetBlobName(blobPath);
            var blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.FetchAttributesAsync();
            long blobLength = blockBlob.Properties.Length;

            long blobLengthRemaining = blobLength;
            long segmentSize = 1 * 1024 * 1024 / 4; //0.25 MB chunk
            long startPosition = 0;
            using (var ms = new MemoryStream())
            {
                do
                {
                    long blockSize = Math.Min(segmentSize, blobLengthRemaining);
                    await blockBlob.DownloadRangeToStreamAsync(ms, startPosition, blockSize);

                    startPosition += blockSize;
                    blobLengthRemaining -= blockSize;

                    DownloadProgressChanged?.Invoke(null, new DownloadProgressChangedEventArgs(blobLength, startPosition));
                } while (blobLengthRemaining > 0);
                
                return ms.ToArray();
            }
        }

        public async Task<long> FetchFileLengthAsync(string blobPath)
        {
            var containerName = GetContainerName(blobPath);
            var container = blobClient.GetContainerReference(containerName);

            var blobName = GetBlobName(blobPath);
            var blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.FetchAttributesAsync();
            return blockBlob.Properties.Length;
        }

        private string GetContainerName(string blobPath)
            => blobPath.Split("/")[1];

        private string GetBlobName(string blobPath)
            => blobPath.Replace($"/{GetContainerName(blobPath)}/", "");
    }
}
