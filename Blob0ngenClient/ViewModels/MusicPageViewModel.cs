using Blob0ngenClient.Views.Controls;
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

        private List<DownloadedTrack> downloadedTracks;
        private List<Music> defaultTracks;


        public MusicPageViewModel(IFolderDialogService folderDialogService, IDatabaseAccess db)
        {
            this.folderDialogService = folderDialogService;
            this.db = db;

            defaultTracks = this.db.ReadMusic().ToList();
            Tracks = defaultTracks;
            Albums = defaultTracks.ToAlbums().ToList();

            downloadedTracks = DownloadedTrackAccess.Get();

            OrderComboBoxItems = new ConditionsComboBoxItemList()
            {
                new OrderConditionsComboBoxItem("追加された日付", x => x.UploadedDate),
                new OrderConditionsComboBoxItem("名前順", x => x.Title, x => x.Album),
                new OrderConditionsComboBoxItem("リリース年", x => x.Date),
                new OrderConditionsComboBoxItem("アーティスト", x => x.Artist, x => x.AlbumArtist),
            };
            SelectedOrderComboBoxItem = OrderComboBoxItems.First() as OrderConditionsComboBoxItem;

            FilterComboBoxItems = new ConditionsComboBoxItemList()
            {
                new FilterConditionsComboBoxItem("すべて", x => true),
                new FilterConditionsComboBoxItem("ダウンロード済み", x => downloadedTracks.Any(y => y.TrackId == x.ID)),
                new FilterConditionsComboBoxItem("未ダウンロード", x => !downloadedTracks.Any(y => y.TrackId == x.ID)),
            };
            SelectedFilterComboBoxItem = FilterComboBoxItems.First() as FilterConditionsComboBoxItem;
        }

        #region class ConditionsComboBoxItem
        public class ConditionsComboBoxItem<T> : IConditionsComboBoxItem
        {
            public string DisplayName { get; set; }
            public Func<Music, T> TrackFunction { get; set; }
            public Func<Music, T> AlbumFunction { get; set; }

            public ConditionsComboBoxItem(string displayName, Func<Music, T> function)
                : this(displayName, function, function) { }

            public ConditionsComboBoxItem(string displayName, Func<Music, T> trackFunction, Func<Music, T> albumFunction)
            {
                DisplayName = displayName;
                TrackFunction = trackFunction;
                AlbumFunction = albumFunction;
            }
        }

        public class FilterConditionsComboBoxItem : ConditionsComboBoxItem<bool>
        {
            public FilterConditionsComboBoxItem(string displayName, Func<Music, bool> function)
                : base(displayName, function) { }

            public FilterConditionsComboBoxItem(string displayName, Func<Music, bool> trackFunction, Func<Music, bool> albumFunction)
                : base(displayName, trackFunction, albumFunction) { }
        }

        public class OrderConditionsComboBoxItem : ConditionsComboBoxItem<object>
        {
            public OrderConditionsComboBoxItem(string displayName, Func<Music, object> function)
                : base(displayName, function) { }

            public OrderConditionsComboBoxItem(string displayName, Func<Music, object> trackFunction, Func<Music, object> albumFunction)
                : base(displayName, trackFunction, albumFunction) { }
        }
        #endregion

        #region FilterComboBoxItems 変更通知プロパティ
        private ConditionsComboBoxItemList filterComboBoxItems;
        public ConditionsComboBoxItemList FilterComboBoxItems
        {
            get { return filterComboBoxItems; }
            set { this.SetProperty(ref filterComboBoxItems, value); }
        }
        #endregion

        #region OrderComboBoxItems 変更通知プロパティ
        private ConditionsComboBoxItemList orderComboBoxItems;
        public ConditionsComboBoxItemList OrderComboBoxItems
        {
            get { return orderComboBoxItems; }
            set { this.SetProperty(ref orderComboBoxItems, value); }
        }
        #endregion

        #region SelectedOrderComboBoxItem 変更通知プロパティ
        private OrderConditionsComboBoxItem selectedOrderComboBoxItem;
        public OrderConditionsComboBoxItem SelectedOrderComboBoxItem
        {
            get { return selectedOrderComboBoxItem; }
            set
            {
                this.SetProperty(ref selectedOrderComboBoxItem, value);
                if (SelectedFilterComboBoxItem == null)
                {
                    Tracks = Tracks.OrderBy(value.TrackFunction).ToList();
                    Albums = Tracks.OrderBy(value.AlbumFunction).ToAlbums().ToList();
                }
                else
                {
                    Tracks = Tracks.Where(SelectedFilterComboBoxItem.TrackFunction)
                                .OrderBy(value.TrackFunction).ToList();
                    Albums = Tracks.Where(SelectedFilterComboBoxItem.AlbumFunction)
                                .OrderBy(value.AlbumFunction).ToAlbums().ToList();
                }
            }
        }
        #endregion

        #region SelectedFilterComboBoxItem 変更通知プロパティ
        private FilterConditionsComboBoxItem selectedFilterComboBoxItem;
        public FilterConditionsComboBoxItem SelectedFilterComboBoxItem
        {
            get { return selectedFilterComboBoxItem; }
            set
            {
                this.SetProperty(ref selectedFilterComboBoxItem, value);
                if (SelectedOrderComboBoxItem == null)
                {
                    Tracks = defaultTracks.Where(value.TrackFunction).ToList();
                    Albums = defaultTracks.Where(value.AlbumFunction).ToAlbums().ToList();
                }
                else
                {
                    Tracks = defaultTracks.Where(value.TrackFunction)
                                .OrderBy(SelectedOrderComboBoxItem.TrackFunction).ToList();
                    Albums = defaultTracks.Where(value.AlbumFunction)
                                .OrderBy(SelectedOrderComboBoxItem.AlbumFunction).ToAlbums().ToList();
                }
            }
        }
        #endregion

        
        private ICommand albumDownloadButtonClickCommand;
        public ICommand AlbumDownloadButtonClickCommand
            => albumDownloadButtonClickCommand ?? 
            (albumDownloadButtonClickCommand = new DelegateCommand<Music>(OnAlbumDownloadButtonClick));

        private async void OnAlbumDownloadButtonClick(Music music)
        {
            var folder = await folderDialogService.PickSingleFolderAsync();
            if (folder != null)
            {
                foreach (var m in Tracks.Where(x => x.Album == music.Album))
                {
                    App.DownloadManager.ReserveDownloadAsync(m, folder);
                }
            }
        }


        private ICommand buttonClickCommand;
        public ICommand ButtonClickCommand
            => buttonClickCommand ?? (buttonClickCommand = new DelegateCommand<Music>(OnButtonClick));

        private async void OnButtonClick(Music music)
        {
            var folder = await folderDialogService.PickSingleFolderAsync();
            if (folder != null)
            {
                App.DownloadManager.ReserveDownloadAsync(music, folder);
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
