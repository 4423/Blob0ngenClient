using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace Blob0ngenClient.Models
{
    public static class MyApplicationData
    {
        private static IPropertySet SettingsValues 
            => ApplicationData.Current.LocalSettings.Values;


        public static bool IsCreateFolderOn
        {
            get => SettingsValues.ContainsKey("IsCreateFolderOn") ? (bool)SettingsValues["IsCreateFolderOn"] : false;
            set => SettingsValues["IsCreateFolderOn"] = value;
        }

        public static string SasUri
        {
            get => SettingsValues["SasUri"] as string;
            set => SettingsValues["SasUri"] = value;
        }

        public static string SqlConnectionString
        {
            get => SettingsValues["SqlConnectionString"] as string;
            set => SettingsValues["SqlConnectionString"] = value;
        }
    }
}
