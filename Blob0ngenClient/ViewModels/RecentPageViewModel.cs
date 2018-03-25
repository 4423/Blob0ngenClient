using Blob0ngenClient.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blob0ngenClient.ViewModels
{
    public class RecentPageViewModel : BindableBase
    {
        private IDatabaseAccess db;

        public RecentPageViewModel(IDatabaseAccess db)
        {
            this.db = db;
            var tracks = db.ReadMusic();
            if (tracks.Count() > 0)
            {
                DownloadedTracks = new List<Music>();
                foreach (var log in DownloadedTrackAccess.Get().OrderBy(x => x.DownloadedDateTime))
                {
                    var track = tracks.SingleOrDefault(x => x.ID == log.TrackId);
                    if (track != null)
                    {
                        DownloadedTracks.Add(track);
                    }
                }
            }
        }

        private List<Music> downloadedTracks;
        public List<Music> DownloadedTracks
        {
            get { return downloadedTracks; }
            set { this.SetProperty(ref downloadedTracks, value); }
        }
    }
}
