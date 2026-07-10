using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeBioSecurityResponseModel
    {
        public string BeeBioCode { get; set; } = null!;
        public string? BeeBioDescription { get; set; }
    }
}
