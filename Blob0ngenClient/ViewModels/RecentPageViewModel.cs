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
        public RecentPageViewModel(IDatabaseAccess db)
        {
            var log = DownloadedTrackAccess.Get();
            DownloadedTracks = log.Join(db.ReadMusic(), l => l.TrackId, m => m.ID, (l, m) => new
            {
                DateTime = l.DownloadedDateTime,
                Track = m
            }).OrderByDescending(x => x.DateTime).Select(x => x.Track).ToList();
        }

        private List<Music> downloadedTracks;
        public List<Music> DownloadedTracks
        {
            get { return downloadedTracks; }
            set { this.SetProperty(ref downloadedTracks, value); }
        }
    }
}
