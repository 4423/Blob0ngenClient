using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blob0ngenClient.Models
{
    public class Music
    {
        public int ID { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string AlbumArtist { get; set; }
        public string Date { get; set; }
        public int TrackNumber { get; set; }
        public string BlobPath { get; set; }
        public string CoverArtPath { get; set; }
        public DateTime UploadedDate { get; set; }
    }

    public class Album
    {
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public string CoverArtPath { get; set; }
    }
}
