using Blob0ngenClient.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Blob0ngenClient.ViewModels
{
    public class ShellPageViewModel : BindableBase
    {
        public ShellPageViewModel()
        {
        }
        

        #region ダウンロード中の情報に関する変更通知プロパティ
        private string title;
        public string Title { get { return title; } set { this.SetProperty(ref title, value); } }

        private string artist;
        public string Artist { get { return artist; } set { this.SetProperty(ref artist, value); } }

        private string imageUrl;
        public string ImageUrl { get { return imageUrl; } set { this.SetProperty(ref imageUrl, value); } }
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

        private void OnDownloadAbortButtonClick()
        {

        }
    }
}
