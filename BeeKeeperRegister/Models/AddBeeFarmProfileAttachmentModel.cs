using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class AddBeeFarmProfileAttachmentModel
    {
        public string LocationId { get; set; }
        public string AttachmentCode { get; set; }
        public string AttachmentName { get; set; }
        public FileStream FileDocument { get; set; }
    }
}
