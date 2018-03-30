using Blob0ngenClient.Models;
using Blob0ngenClient.Views;
using Prism.Unity.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Blob0ngenClient
{
    sealed partial class App : PrismUnityApplication
    {
        public static IDatabaseAccess DatabaseAccess { get; private set; }
        public static IFolderDialogService FolderDialogService { get; private set; }
        public static IDialogService DialogService { get; private set; }

        public static MusicDownloadManager DownloadManager { get; } = new MusicDownloadManager();


        public App()
        {
#if DEBUG
            DatabaseAccess = new Tests.DummyAccess();
#else
            if (!String.IsNullOrEmpty(MyApplicationData.SqlConnectionString))
            {
                DatabaseAccess = new SqlAccess(MyApplicationData.SqlConnectionString);
            }
            else
            {
                DatabaseAccess = new EmptyDatabaseAccess();
            }
#endif
            FolderDialogService = new FolderDialogService();
            DialogService = new ContentDialogService();

            this.InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = false;

            NavigationService.Navigate("Shell", null);
            return Task.CompletedTask;
        }
    }
}
