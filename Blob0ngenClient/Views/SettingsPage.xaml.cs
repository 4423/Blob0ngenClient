using Blob0ngenClient.Models;
using Blob0ngenClient.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPageViewModel ViewModel { get; }
#if DEBUG
            = new SettingsPageViewModel(new Tests.DummyAccess(), new ContentDialogService());
#else
            = new SettingsPageViewModel(new SqlAccess(App.SqlDatabaseConnectionString), new ContentDialogService());
#endif

        public SettingsPage()
        {
            this.InitializeComponent();
        }
    }
}
