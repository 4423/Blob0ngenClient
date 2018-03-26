using Blob0ngenClient.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace Blob0ngenClient.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPageViewModel ViewModel { get; } = new ShellPageViewModel();

        public ShellPage()
        {
            string url = ResourceLoader.GetForCurrentView().GetString("DefaultCoverArtUrl");
            Resources.Add("DefaultCoverArtUrl", url);

            this.InitializeComponent();

            ContentFrame.Navigate(typeof(MusicPage));
            NavView.SelectedItem = NavView.MenuItems.First();
        }

        private void NavViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag)
                {
                    case "music":
                        ContentFrame.Navigate(typeof(MusicPage));
                        break;
                    case "recent":
                        ContentFrame.Navigate(typeof(RecentPage));
                        break;
                }
            }
        }
    }
}
