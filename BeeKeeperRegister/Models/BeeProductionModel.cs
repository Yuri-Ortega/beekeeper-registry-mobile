using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class BeeProductionModel
    {
        public string BeeProdId { get; set; } = null!;
        public string BeeProductionDescription { get; set; } = null!;
    }
}
