using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blob0ngenClient.Models
{
    public class EmptyDatabaseAccess : IDatabaseAccess
    {
        public IEnumerable<Music> ReadMusic()
        {
            return new List<Music>();
        }
    }
}
