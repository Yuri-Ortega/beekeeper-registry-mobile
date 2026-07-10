using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeProductionSystemResponseModel
    {
        public string BeeProSysId { get; set; } = null!;
        public string? BeeSystemProduction { get; set; }
    }
}
