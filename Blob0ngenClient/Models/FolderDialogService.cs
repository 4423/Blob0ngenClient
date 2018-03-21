using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Blob0ngenClient.Models
{
    public class FolderDialogService : IFolderDialogService
    {
        public async Task<StorageFolder> PickSingleFolderAsync()
        {
            var folderPicker = new FolderPicker()
            {
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            folderPicker.FileTypeFilter.Add("*");
            return await folderPicker.PickSingleFolderAsync();
        }
    }
}
