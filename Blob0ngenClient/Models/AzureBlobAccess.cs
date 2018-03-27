using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Blob0ngenClient.Models
{
    public static class AzureBlobAccess
    {
        private static string accountName = ResourceLoader.GetForCurrentView().GetString("AzureBlobAccountName");
        private static string sas = ResourceLoader.GetForCurrentView().GetString("AzureBlobSAS");

        public static Uri GetBlobSasUri(string blobPath)
            => new Uri($"https://{accountName}.blob.core.windows.net{Uri.UnescapeDataString(blobPath)}?{sas}");
    }
}
