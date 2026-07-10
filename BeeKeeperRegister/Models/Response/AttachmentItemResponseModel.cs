using BeeKeeperRegister.Handler;
using DevExpress.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class AttachmentItemResponseModel
    {
        public string? FileName { get; set; }
        public string FileIcon => FileHandler.GetFileIcon(FileName!);
    }
}
