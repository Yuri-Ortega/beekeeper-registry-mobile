using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class RegionResponseModel
    {
        public string Rcode { get; set; } = null!;

        public string? Region { get; set; }
    }
}
