using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class AddBeeFarmProfileAttachmentRequestModel
    {
        public required string LocationId { get; set; }
        public required string AttachmentCode { get; set; } = null!;
        public required string AttachmentName { get; set; }
        public required FileStream FileDocument { get; set; }
    }
}
