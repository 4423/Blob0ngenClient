using LiteDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Blob0ngenClient.Models
{
    public static class DownloadedTrackAccess
    {
        private static readonly string ConnectionString =
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "Data.db");

        private const string TableName = "Tracks";


        public static void Insert(DownloadedTrack track)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var tracks = db.GetCollection<DownloadedTrack>(TableName);
                tracks.Insert(track);
            }
        }

        public static List<DownloadedTrack> Get()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var tracks = db.GetCollection<DownloadedTrack>(TableName);
                return tracks.FindAll().ToList();
            }
        }
    }

    public class DownloadedTrack
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public DateTime DownloadedDateTime { get; set; }

        public DownloadedTrack() { }

        public DownloadedTrack(int trackId)
        {
            this.TrackId = trackId;
        }

        public DownloadedTrack(int trackId, DateTime downloadedDateTime) : this(trackId)
        {
            this.DownloadedDateTime = downloadedDateTime;
        }
    }
}
