using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blob0ngenClient.Helpers
{
    public static class PathExtensions
    {
        public static string ToValidFileName(this string desiredName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Concat(desiredName.Select(c => invalidChars.Contains(c) ? '_' : c));
        }
    }
}
