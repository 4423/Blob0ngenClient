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
    public class SettingsPageViewModel : BindableBase
    {
        private IDatabaseAccess db;
        private IDialogService dialogService;

        public SettingsPageViewModel(IDatabaseAccess db, IDialogService dialogService)
        {
            this.db = db;
            this.dialogService = dialogService;
        }


        private ICommand deleteLogButtonClickCommand;
        public ICommand DeleteLogButtonClickCommand
            => deleteLogButtonClickCommand ?? (deleteLogButtonClickCommand = new DelegateCommand(DeleteLog));

        private void DeleteLog()
        {
            DownloadedTrackAccess.DeleteAll();
            if (DownloadedTrackAccess.Get().Count == 0)
            {
                dialogService.ShowDialogAsync("成功", "すべてのダウンロード履歴を削除しました。");
            }
            else
            {
                dialogService.ShowDialogAsync("失敗", "すべてのダウンロード履歴の削除に失敗しました。");
            }
        }


        private ICommand registerLogButtonClickCommand;
        public ICommand RegisterLogButtonClickCommand
            => registerLogButtonClickCommand ?? (registerLogButtonClickCommand = new DelegateCommand(RegisterLog));

        private void RegisterLog()
        {
            foreach (var music in db.ReadMusic())
            {
                DownloadedTrackAccess.Insert(new DownloadedTrack(music.ID));
            }

            if (DownloadedTrackAccess.Get().Count >= db.ReadMusic().Count())
            {
                dialogService.ShowDialogAsync("成功", "すべての楽曲をダウンロード済みに登録しました。");
            }
            else
            {
                dialogService.ShowDialogAsync("失敗", "すべての楽曲をダウンロード済みに登録することができませんでした。");
            }
        }


        #region IsCreateFolderOn 変更通知プロパティ
        private bool isCreateFolderOn = MyApplicationData.IsCreateFolderOn;
        public bool IsCreateFolderOn
        {
            get { return isCreateFolderOn; }
            set
            {
                this.SetProperty(ref isCreateFolderOn, value);
                MyApplicationData.IsCreateFolderOn = value;
            }
        }
        #endregion


        #region SasUri 変更通知プロパティ
        private string sasUri = MyApplicationData.SasUri;
        public string SasUri
        {
            get { return sasUri; }
            set
            {
                this.SetProperty(ref sasUri, value);
                MyApplicationData.SasUri = value;
            }
        }
        #endregion

        public string SasUriPlaceholder
            => "https://myaccount.blob.core.windows.net/sascontainer/sasblob.txt?sv=2015-04-05&st=2015-04-29T22%3A18%3A26Z&se=2015-04-30T02%3A23%3A26Z&sr=b&sp=rw&sip=168.1.5.60-168.1.5.70&spr=https&sig=Z%2FRHIX5Xcg0Mq2rqI3OlWTjEg2tYkboXr1P9ZUXDtkk%3D";
    }
}
