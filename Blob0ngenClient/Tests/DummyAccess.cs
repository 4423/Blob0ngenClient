using Blob0ngenClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Blob0ngenClient.Tests
{
    public class DummyAccess : IDatabaseAccess
    {
        public IEnumerable<Music> ReadMusic()
        {
            yield return new Music()
            {
                ID = 0,
                Title = "Blooming Lily",
                Artist = "澤村・スぺンサー・英梨々 (CV.大西沙織)",
                AlbumArtist = "澤村・スぺンサー・英梨々 (CV.大西沙織)",
                Album = "冴えない彼女の育てかた キャラクターイメージソング 澤村・スペンサー・英梨々",
                Date = "2014",
                TrackNumber = 1,
                CoverArtPath = "https://www.aniplexplus.com/res/Yobd1B?h=600",
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 1,
                Title = "LOVE iLLUSiON (Eriri Solo Ver.)",
                Artist = "澤村・スぺンサー・英梨々 (CV.大西沙織)",
                AlbumArtist = "澤村・スぺンサー・英梨々 (CV.大西沙織)",
                Album = "冴えない彼女の育てかた キャラクターイメージソング 澤村・スペンサー・英梨々",
                Date = "2014",
                TrackNumber = 2,
                CoverArtPath = "https://www.aniplexplus.com/res/Yobd1B?h=600",
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 2,
                Title = "M♭",
                Artist = "加藤恵 (CV.安野希世乃)",
                AlbumArtist = "加藤恵 (CV.安野希世乃)",
                Album = "冴えない彼女の育てかた キャラクターイメージソング 加藤恵",
                Date = "2014",
                TrackNumber = 1,
                CoverArtPath = "https://www.aniplexplus.com/res/CBGEBy?h=600",                
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 3,
                Title = "LOVE iLLUSiON (Megumi Solo Ver.)",
                Artist = "加藤恵 (CV.安野希世乃)",
                AlbumArtist = "加藤恵 (CV.安野希世乃)",
                Album = "冴えない彼女の育てかた キャラクターイメージソング 加藤恵",
                Date = "2014",
                TrackNumber = 2,
                CoverArtPath = "https://www.aniplexplus.com/res/CBGEBy?h=600",                
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 4,
                Title = "饒舌スキャンダラス",
                Artist = "霞ヶ丘詩羽 (CV.茅野愛衣)",
                AlbumArtist = "霞ヶ丘詩羽 (CV.茅野愛衣)",
                Album = "冴えない彼女の育てかた キャラクターイメージソング 霞ヶ丘詩羽",
                Date = "2014",
                TrackNumber = 1,
                CoverArtPath = "https://www.aniplexplus.com/res/QMFxSU?h=600",
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 5,
                Title = "LOVE iLLUSiON (Utaha Solo Ver.)",
                Artist = "霞ヶ丘詩羽 (CV.茅野愛衣)",
                AlbumArtist = "霞ヶ丘詩羽 (CV.茅野愛衣)",
                Album = "冴えない彼女の育てかた キャラクターイメージソング 霞ヶ丘詩羽",
                Date = "2014",
                TrackNumber = 2,
                CoverArtPath = "https://www.aniplexplus.com/res/QMFxSU?h=600",
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 6,
                Title = "DOUBLE RAINBOW DREAMS",
                Artist = "澤村・スペンサー・英梨々 (CV.大西沙織) & 霞ヶ丘詩羽 (CV.茅野愛衣)",
                AlbumArtist = "冴えない彼女の育てかた",
                Album = "冴えない彼女の育てかた Character Song Collection",
                Date = "2017",
                TrackNumber = 12,
                CoverArtPath = null,
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 7,
                Title = "Cherish you",
                Artist = "氷堂美智留(CV.矢作紗友里)",
                AlbumArtist = "冴えない彼女の育てかた",
                Album = "冴えない彼女の育てかた Character Song Collection",
                Date = "2017",
                TrackNumber = 8,
                CoverArtPath = null,
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
            yield return new Music()
            {
                ID = 8,
                Title = "Daydreamer",
                Artist = "氷堂美智留(CV.矢作紗友里)",
                AlbumArtist = "冴えない彼女の育てかた",
                Album = "冴えない彼女の育てかた Character Song Collection",
                Date = "2017",
                TrackNumber = 9,
                CoverArtPath = "https://www.aniplexplus.com/res/9wONtJ?h=600",
                BlobPath = "/image/dummy/dummy.png",
                UploadedDate = DateTime.Now
            };
        }
    }
}
