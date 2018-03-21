using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blob0ngenClient.Models
{
    public class SqlAccess : IDatabaseAccess
    {
        private string connectionString;

        public SqlAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<Music> ReadMusic()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var text = "SELECT * FROM Musics";
                using (var cmd = new SqlCommand(text, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        yield return new Music()
                        {
                            ID = (int)reader["ID"],
                            Title = reader["Title"] as string,
                            Album = reader["Album"] as string,
                            Artist = reader["Artist"] as string,
                            AlbumArtist = reader["AlbumArtist"] as string,
                            Date = reader["Date"] as string,
                            TrackNumber = (int)reader["TrackNumber"],
                            BlobPath = reader["BlobPath"] as string,
                            CoverArtPath = reader["CoverArtPath"] as string,
                            UploadedDate = (DateTime)reader["UploadedDate"]
                        };
                    }
                }
            }
        }
    }
}
