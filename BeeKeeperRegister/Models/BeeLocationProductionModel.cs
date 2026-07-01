using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class BeeLocationProductionModel
    {
        public string BeeProdId { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string LocationId { get; set; } = null!;
        public string BeeProductionDescription { get; set; } = null!;
        public int? Production { get; set; }
        public string? Remarks { get; set; }
    }
}
