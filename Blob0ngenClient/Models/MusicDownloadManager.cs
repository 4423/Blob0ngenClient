using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace Blob0ngenClient.Models
{
    public delegate void DownloadProgressEventHandler(BackgroundDownloadProgress progress, Music music);

    public delegate void DownloadQueueEventHandler(int count);
    
    public class MusicDownloadManager
    {
        public event DownloadProgressEventHandler ProgressEventHandler;
        public event DownloadQueueEventHandler QueueStatusChanged;

        private Queue<(Music Music, StorageFolder DestinationFolder, string FileName)> queue
            = new Queue<(Music Music, StorageFolder DestinationFolder, string FileName)>();

        private Progress<DownloadOperation> progressCallback;


        public MusicDownloadManager()
        {
            progressCallback = new Progress<DownloadOperation>(ProgressCallback);
        }


        private void Cancel(DownloadOperation download)
            => download.AttachAsync().Cancel();

        public async Task CancelAsync()
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (downloads.Count > 0)
            {
                Cancel(downloads.Last());
            }
        }

        public async Task CancelAllAsync()
        {
            foreach (var download in await BackgroundDownloader.GetCurrentDownloadsAsync())
            {
                Cancel(download);
            }            
        }


        public async Task ReserveDownloadAsync(Music music, StorageFolder destinationFolder)
            => await ReserveDownloadAsync(music, destinationFolder, Path.GetFileName(Uri.UnescapeDataString(music.BlobPath)));
        
        public async Task ReserveDownloadAsync(Music music, StorageFolder destinationFolder, string filename)
        {
            Enqueue(queue, (music, destinationFolder, filename));
            if (queue.Count == 1)
            {
                await StartDownloadAsync();
            }
        }


        private async Task StartDownloadAsync()
        {
            var (music, folder, filename) = queue.First();

            var uri = AzureBlobAccess.GetBlobSasUri(music.BlobPath);
            var destFile = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(uri, destFile);
            try
            {
                await download.StartAsync().AsTask(progressCallback);
            }
            catch (Exception e)
            when (e.HResult == -2147012879 ||   // Canceled
                  e.HResult == -2145844845)     // 403 Forbidden
            {
                ProgressCallback(download);
                await destFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
        }


        private async void ProgressCallback(DownloadOperation operation)
        {
            ProgressEventHandler?.Invoke(operation.Progress, queue.FirstOrDefault().Music);

            if (IsDownloadEnd(operation.Progress))
            {
                await StartNextDownloadAsync();
            }
        }

        private async Task StartNextDownloadAsync()
        {
            Dequeue(queue);
            if (queue.Count > 0)
            {
                await StartDownloadAsync();
            }
        }

        private bool IsDownloadEnd(BackgroundDownloadProgress progress)
        {
            if (progress.BytesReceived == progress.TotalBytesToReceive)
                return true;
            
            switch (progress.Status)
            {
                case BackgroundTransferStatus.Completed:
                case BackgroundTransferStatus.Canceled:
                case BackgroundTransferStatus.Error:
                    return true;
            }
            return false;
        }


        private (Music Music, StorageFolder DestinationFolder, string FileName) Dequeue
            (Queue<(Music Music, StorageFolder DestinationFolder, string FileName)> queue)
        {
            var t = queue.Dequeue();
            QueueStatusChanged?.Invoke(queue.Count);
            return t;
        }

        private void Enqueue(Queue<(Music Music, StorageFolder DestinationFolder, string FileName)> queue,
            (Music Music, StorageFolder DestinationFolder, string FileName) item)
        {
            queue.Enqueue(item);
            QueueStatusChanged?.Invoke(queue.Count);
        }
    }
}
