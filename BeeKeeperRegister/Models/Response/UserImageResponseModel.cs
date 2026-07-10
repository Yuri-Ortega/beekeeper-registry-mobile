using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class UserImageResponseModel
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public byte[]? UploadValidId { get; set; }
        public byte[]? Signature { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
