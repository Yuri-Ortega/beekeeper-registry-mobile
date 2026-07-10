using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeCommonDiseasesResponseModel
    {
        public string CommonDiseaseBee { get; set; } = null!;
        public string? CommonDiseaseBeeDescription { get; set; }
    }
}
