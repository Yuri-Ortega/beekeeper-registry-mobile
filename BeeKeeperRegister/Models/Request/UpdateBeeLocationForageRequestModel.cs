using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class UpdateBeeLocationForageRequestModel
    {
        public required string LocationID { get; set; }
        public required string OldForageCode { get; set; } = null!;
        public required string ForageCode { get; set; } = null!;
        public required string? ForagesDescription { get; set; }
    }
}
