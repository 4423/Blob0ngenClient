using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Blob0ngenClient.Models
{
    public class ContentDialogService : IDialogService
    {
        public async Task ShowDialogAsync(string title, string content, string closeButtonText = "Ok")
        {
            ContentDialog noWifiDialog = new ContentDialog()
            {
                Title = title,
                Content = content,
                CloseButtonText = closeButtonText
            };

            await noWifiDialog.ShowAsync();
        }
    }
}
