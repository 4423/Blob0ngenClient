using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Blob0ngenClient.Models
{
    public static class FileDownloadHelper
    {
        private static async Task Download(
            StorageFolder folder, string srcBlobPath,
            EventHandler<DownloadProgressChangedEventArgs> eventHandler)
        {
            var filename = Path.GetFileName(srcBlobPath);
            var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            var blobAccess = new AzureBlobAccess();
            blobAccess.DownloadProgressChanged += eventHandler;

            var bytes = await blobAccess.DownloadBytesAsync(srcBlobPath);
            await FileIO.WriteBytesAsync(file, bytes);
        }

        public static async Task<bool> DownloadToSelectedFolderAsync(
            IFolderDialogService folderDialog, string srcBlobPath,
            EventHandler<DownloadProgressChangedEventArgs> eventHandler)
        {
            var folder = await folderDialog.PickSingleFolderAsync();
            if (folder == null)
            {
                return true;
            }

            await Download(folder, srcBlobPath, eventHandler);
            return false;
        }

        public static async Task<bool> DownloadToSelectedFolderAsync(
            IFolderDialogService folderDialog, List<string> srcBlobPathList,
            EventHandler<DownloadProgressChangedEventArgs> eventHandler)
        {
            var folder = await folderDialog.PickSingleFolderAsync();
            if (folder == null)
            {
                return true;
            }

            var blobAccess = new AzureBlobAccess();

            long sumOfLength = 0;
            foreach (var srcBlobPath in srcBlobPathList)
            {
                sumOfLength += await blobAccess.FetchFileLengthAsync(srcBlobPath);
            }

            long downloadedLength = 0;
            foreach (var srcBlobPath in srcBlobPathList)
            {
                EventHandler<DownloadProgressChangedEventArgs> handler = (_, e) => 
                {
                    var len = downloadedLength + e.DownloadedSize;
                    eventHandler?.Invoke(null, new DownloadProgressChangedEventArgs(sumOfLength, len));
                    if (e.IsCompleted)
                    {
                        downloadedLength += e.DownloadedSize;
                    }
                };
                await Download(folder, srcBlobPath, handler);
            }            

            return false;
        }
    }
}
