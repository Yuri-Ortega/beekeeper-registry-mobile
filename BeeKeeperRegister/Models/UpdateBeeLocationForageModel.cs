using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class UpdateBeeLocationForageModel
    {
        public string LocationID { get; set; }
        public string OldForageCode { get; set; } = null!;
        public string ForageCode { get; set; } = null!;
        public string? ForagesDescription { get; set; }
    }
}
