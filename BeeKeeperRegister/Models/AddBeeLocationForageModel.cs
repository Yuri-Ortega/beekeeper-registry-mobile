using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class AddBeeLocationForageModel
    {
        public string LocationId { get; set; }
        public string ForageCode { get; set; } = null!;
        public string? ForagesDescription { get; set; }
    }
}
