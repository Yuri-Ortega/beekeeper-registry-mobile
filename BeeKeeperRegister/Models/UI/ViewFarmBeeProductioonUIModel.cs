using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.UI
{
    public class ViewFarmBeeProductioonUIModel
    {
        public string LocationId { get; set; } = string.Empty;
        public string BeeProdID { get; set; } = string.Empty;
        public string BeeProduction { get; set; } = string.Empty;
        public int? EstProdYield { get; set; }
    }
}
