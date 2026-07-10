using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class AttachmentLibraryResponseModel
    {
        public string AttachmentCode { get; set; } = null!;
        public string? AttachmentName { get; set; }
    }
}
