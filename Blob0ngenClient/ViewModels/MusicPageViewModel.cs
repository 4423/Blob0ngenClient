using Blob0ngenClient.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Blob0ngenClient.ViewModels
{
    public class MusicPageViewModel : BindableBase
    {
        private IFolderDialogService folderDialogService;
        private IDatabaseAccess db;

        public MusicPageViewModel(IFolderDialogService folderDialogService, IDatabaseAccess db)
        {
            this.folderDialogService = folderDialogService;
            this.db = db;

            defaultTracks = this.db.ReadMusic().ToList();
            Tracks = defaultTracks;
            defaultAlbums = Tracks.GroupBy(x => x.Album).Select(x => x.First()).ToList();
            Albums = defaultAlbums;

            KeyValuePair<string, Func<IEnumerable<Music>>> Add(string str, Func<IEnumerable<Music>> func) 
                => new KeyValuePair<string, Func<IEnumerable<Music>>>(str, func);

            OrderItems = new List<KeyValuePair<string, Func<IEnumerable<Music>>>>()
            {
                Add("追加された日付", () => Albums.OrderBy(x => x.UploadedDate)),
                Add("名前順", () => Albums.OrderBy(x => x.Album)),
                Add("リリース年", () => Albums.OrderBy(x => x.Date)),
                Add("アーティスト", () => Albums.OrderBy(x => x.AlbumArtist))
            };
            SelectedOrderItem = OrderItems.First();
            
            FilterItems = new List<KeyValuePair<string, Func<IEnumerable<Music>>>>()
            {
                Add("すべて", () => defaultAlbums),
                Add("ダウンロード済み", () => defaultAlbums.Where(x => downloadedItems.Contains(x.ID))),
                Add("未ダウンロード", () => defaultAlbums.Where(x => !downloadedItems.Contains(x.ID)))
            };
            SelectedFilterItem = FilterItems.First();

            
            TrackOrderItems = new List<KeyValuePair<string, Func<IEnumerable<Music>>>>()
            {
                Add("追加された日付", () => Tracks.OrderBy(x => x.UploadedDate)),
                Add("名前順", () => Tracks.OrderBy(x => x.Title)),
                Add("リリース年", () => Tracks.OrderBy(x => x.Date)),
                Add("アーティスト", () => Tracks.OrderBy(x => x.Artist))
            };
            SelectedTrackOrderItem = SelectedOrderItem;

            TrackFilterItems = new List<KeyValuePair<string, Func<IEnumerable<Music>>>>()
            {
                Add("すべて", () => defaultTracks),
                Add("ダウンロード済み", () => defaultTracks.Where(x => downloadedItems.Contains(x.ID))),
                Add("未ダウンロード", () => defaultTracks.Where(x => !downloadedItems.Contains(x.ID)))
            };
            SelectedTrackFilterItem = SelectedFilterItem;
        }


        private bool isOpenDownloadPanel;
        public bool IsOpenDownloadPanel
        {
            get { return isOpenDownloadPanel; }
            set { this.SetProperty(ref isOpenDownloadPanel, value); }
        }
        

        private Music downloadingMusic;
        public Music DownloadingMusic
        {
            get { return downloadingMusic; }
            set { this.SetProperty(ref downloadingMusic, value); }
        }


        private ICommand albumDownloadButtonClickCommand;
        public ICommand AlbumDownloadButtonClickCommand
            => albumDownloadButtonClickCommand ?? 
            (albumDownloadButtonClickCommand = new DelegateCommand<Music>(OnAlbumDownloadButtonClick));

        private async void OnAlbumDownloadButtonClick(Music music)
        {
            IsOpenDownloadPanel = true;
            DownloadingMusic = new Music()
            {
                Title = music.Album,
                Artist = music.AlbumArtist,
                CoverArtPath = music.CoverArtPath
            };

            var album = Tracks.Where(x => x.Album == music.Album);
            var albumUrls = album.Select(x => Uri.UnescapeDataString(x.BlobPath)).ToList();

            bool isCanceled = await FileDownloadHelper.DownloadToSelectedFolderAsync(
                folderDialogService, albumUrls, BlobDownloadProgressChanged);

            if (isCanceled)
            {
                ResetDownloadProgressPanel();
            }
            else
            {
                var tracks = album.Select(x => new DownloadedTrack(x.ID, DateTime.Now));
                foreach (var track in tracks)
                {
                    DownloadedTrackAccess.Insert(track);
                }
            }
        }


        private ICommand buttonClickCommand;
        public ICommand ButtonClickCommand
            => buttonClickCommand ?? (buttonClickCommand = new DelegateCommand<Music>(OnButtonClick));

        private async void OnButtonClick(Music item)
        {
            IsOpenDownloadPanel = true;
            DownloadingMusic = item;

            var srcUri = Uri.UnescapeDataString(item.BlobPath);
            bool isCanceled = await FileDownloadHelper.DownloadToSelectedFolderAsync(
                folderDialogService, srcUri, BlobDownloadProgressChanged);

            if (isCanceled)
            {
                ResetDownloadProgressPanel();
            }
            else
            {
                DownloadedTrackAccess.Insert(new DownloadedTrack(item.ID, DateTime.Now));
            }
        }

        private async void BlobDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs args)
        {
            DownloadProgressValue = (int)((double)args.DownloadedSize / args.FileSize * DownloadProgressMaximum);
            if (args.IsCompleted)
            {
                await Task.Delay(1000);
                ResetDownloadProgressPanel();
            }
        }

        private void ResetDownloadProgressPanel()
        {
            IsOpenDownloadPanel = false;
            DownloadingMusic = null;
            DownloadProgressValue = 0;
        }

        public int DownloadProgressMaximum => 100;

        private int downloadProgressValue = 0;
        public int DownloadProgressValue
        {
            get { return downloadProgressValue; }
            set { this.SetProperty(ref downloadProgressValue, value); }
        }



        private List<int> downloadedItems = Enumerable.Range(0, 40).ToList();
        private List<Music> defaultTracks;
        private List<Music> defaultAlbums;

        public List<KeyValuePair<string, Func<IEnumerable<Music>>>> TrackOrderItems { get; set; }

        private KeyValuePair<string, Func<IEnumerable<Music>>> selectedTrackOrderItem;
        public KeyValuePair<string, Func<IEnumerable<Music>>> SelectedTrackOrderItem
        {
            get { return selectedTrackOrderItem; }
            set
            {
                this.SetProperty(ref selectedTrackOrderItem, value);
                Tracks = selectedTrackOrderItem.Value?.Invoke().ToList();
            }
        }

        public List<KeyValuePair<string, Func<IEnumerable<Music>>>> TrackFilterItems { get; set; }

        private KeyValuePair<string, Func<IEnumerable<Music>>> selectedTrackFilterItem;
        public KeyValuePair<string, Func<IEnumerable<Music>>> SelectedTrackFilterItem
        {
            get { return selectedTrackFilterItem; }
            set
            {
                this.SetProperty(ref selectedTrackFilterItem, value);
                Tracks = selectedTrackFilterItem.Value?.Invoke().ToList();
            }
        }

        public List<KeyValuePair<string, Func<IEnumerable<Music>>>> OrderItems { get; set; }

        private KeyValuePair<string, Func<IEnumerable<Music>>> selectedOrderItem;
        public KeyValuePair<string, Func<IEnumerable<Music>>> SelectedOrderItem
        {
            get { return selectedOrderItem; }
            set
            {
                this.SetProperty(ref selectedOrderItem, value);
                Albums = selectedOrderItem.Value?.Invoke().ToList();
            }
        }

        public List<KeyValuePair<string, Func<IEnumerable<Music>>>> FilterItems { get; set; }

        private KeyValuePair<string, Func<IEnumerable<Music>>> selectedFilterItem;
        public KeyValuePair<string, Func<IEnumerable<Music>>> SelectedFilterItem
        {
            get { return selectedFilterItem; }
            set
            {
                this.SetProperty(ref selectedFilterItem, value);
                Albums = selectedFilterItem.Value?.Invoke().ToList();
            }
        }

        private List<Music> albums;
        public List<Music> Albums
        {
            get { return albums; }
            set { this.SetProperty(ref albums, value); }
        }

        private List<Music> tracks;
        public List<Music> Tracks
        {
            get { return tracks; }
            set { this.SetProperty(ref tracks, value); }
        }
    }
}
