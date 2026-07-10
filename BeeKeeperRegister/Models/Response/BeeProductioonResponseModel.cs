using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeProductioonResponseModel
    {
        public string BeeProdId { get; set; } = null!;
        public string BeeProductionDescription { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string LocationId { get; set; } = null!;
        public int? EstProdYield { get; set; }
    }
}
