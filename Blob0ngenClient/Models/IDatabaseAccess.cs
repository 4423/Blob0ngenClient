using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blob0ngenClient.Models
{
    public interface IDatabaseAccess
    {
        IEnumerable<Music> ReadMusic();
    }
}
