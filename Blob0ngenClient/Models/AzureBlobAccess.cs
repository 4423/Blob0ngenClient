using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blob0ngenClient.Models
{
    public static class AzureBlobAccess
    {
        private static string AccountName()
        {
            var sasuri = MyApplicationData.SasUri;
            int p1 = sasuri.IndexOf("://") + 3;
            int p2 = sasuri.IndexOf(".blob.core.windows.net");
            return sasuri.Substring(p1, p2 - p1);
        }
        
        private static string Sas()
        {
            var sasuri = MyApplicationData.SasUri;
            int p = sasuri.IndexOf("?") + 1;
            return sasuri.Substring(p, sasuri.Length - p);
        }


        public static Uri GetBlobSasUri(string blobPath)
            => new Uri($"https://{AccountName()}.blob.core.windows.net{Uri.UnescapeDataString(blobPath)}?{Sas()}");
    }
}
