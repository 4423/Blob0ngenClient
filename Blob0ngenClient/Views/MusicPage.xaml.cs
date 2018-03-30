using Blob0ngenClient.Models;
using Blob0ngenClient.Tests;
using Blob0ngenClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Blob0ngenClient.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MusicPage : Page
    {
        public MusicPageViewModel ViewModel { get; }
            = new MusicPageViewModel(App.FolderDialogService, App.DatabaseAccess);

        public MusicPage()
        {
            string url = ResourceLoader.GetForCurrentView().GetString("DefaultCoverArtUrl");
            Resources.Add("defaultCoverArtUrl", url);

            this.InitializeComponent();
        }
    }
}
