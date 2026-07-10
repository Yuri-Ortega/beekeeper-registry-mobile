using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class AddBeeLocationForageRequestModel
    {
        public required string LocationId { get; set; }
        public required string ForageCode { get; set; } = null!;
        public required string? ForagesDescription { get; set; }
    }
}
