using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class BeeLocationForageModel
    {
        public string ClientId { get; set; } = null!;
        public string LocationId { get; set; } = null!;
        public string ForageCode { get; set; } = null!;
        public string? ForagesDescription { get; set; }

    }
}
