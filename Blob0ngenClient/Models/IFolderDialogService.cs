using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Blob0ngenClient.Models
{
    public interface IFolderDialogService
    {
        Task<StorageFolder> PickSingleFolderAsync();
    }
}
