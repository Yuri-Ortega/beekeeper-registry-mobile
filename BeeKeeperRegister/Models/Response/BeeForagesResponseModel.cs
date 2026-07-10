using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeForagesResponseModel
    {
        public string ForageCode { get; set; } = null!;

        public string? ForagesDescription { get; set; }

        public string? ForagesGrpCode { get; set; }

        public string? ForagesGrpDescription { get; set; }
    }
}
