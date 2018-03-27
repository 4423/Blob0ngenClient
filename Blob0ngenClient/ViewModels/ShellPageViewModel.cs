using Blob0ngenClient.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.BackgroundTransfer;

namespace Blob0ngenClient.ViewModels
{
    public class ShellPageViewModel : BindableBase
    {
        public ShellPageViewModel()
        {
            App.DownloadManager.ProgressEventHandler += DownloadProgressEventHandler;
            App.DownloadManager.QueueStatusChanged += DownloadQueueStatusChanged;
        }

        private void DownloadProgressEventHandler(BackgroundDownloadProgress progress, Music music)
        {
            DownloadProgressValue = (int)((double)progress.BytesReceived / progress.TotalBytesToReceive * DownloadProgressMaximum);

            // ダウンロードを完了していれば Completed と定義（もとは Running になっている）
            var status = progress.BytesReceived == progress.TotalBytesToReceive ? 
                            BackgroundTransferStatus.Completed : 
                            progress.Status;
            System.Diagnostics.Debug.WriteLine(status);
            switch (status)
            {
                case BackgroundTransferStatus.Running:
                    DownloadingMusic = music;
                    break;

                case BackgroundTransferStatus.Completed:
                    DownloadedTrackAccess.Insert(new DownloadedTrack(music.ID, DateTime.Now));
                    break;
            }
        }
        
        private async void DownloadQueueStatusChanged(int count)
        {
            System.Diagnostics.Debug.WriteLine(count);
            if (count > 0)
            {
                IsDownloading = true;
            }
            else
            {
                await Task.Delay(1000);
                IsDownloading = false;
                DownloadingMusic = null;
                DownloadProgressValue = 0;
            }
        }


        #region DownloadingMusic 変更通知プロパティ
        private Music downloadingMusic;
        public Music DownloadingMusic
        {
            get { return downloadingMusic; }
            set { this.SetProperty(ref downloadingMusic, value); }
        }
        #endregion
                
        #region IsDownloading 変更通知プロパティ
        private bool isDownloading;
        public bool IsDownloading
        {
            get { return isDownloading; }
            set { this.SetProperty(ref isDownloading, value); }
        }
        #endregion

        public int DownloadProgressMaximum => 100;

        #region DownloadProgressValue 変更通知プロパティ
        private int downloadProgressValue = 0;
        public int DownloadProgressValue
        {
            get { return downloadProgressValue; }
            set { this.SetProperty(ref downloadProgressValue, value); }
        }
        #endregion

        private ICommand downloadAbortButtonClickCommand;
        public ICommand DownloadAbortButtonClickCommand
            => downloadAbortButtonClickCommand ?? (downloadAbortButtonClickCommand = new DelegateCommand(OnDownloadAbortButtonClick));

        private async void OnDownloadAbortButtonClick()
        {
            await App.DownloadManager.CancelAsync();
        }
    }
}
