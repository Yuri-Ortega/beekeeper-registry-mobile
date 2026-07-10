using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class TempDataBeeProductioonModel
    {
        public int Id { get; set; }
        public string? LocationId { get; set; }
        public string? BeeProdId { get; set; }
        public string? BeeProductionDescription { get; set; }
        public int EstProdYield { get; set; }
    }
}
