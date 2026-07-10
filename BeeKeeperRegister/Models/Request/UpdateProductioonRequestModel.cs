using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class UpdateProductioonRequestModel
    {
        public required string OldBeeProdId { get; set; }
        public required string LocationId { get; set; }

        public required string BeeProdId { get; set; }

        public required string BeeProductionDescription { get; set; }

        public required int? EstProdYield { get; set; }
    }
}
