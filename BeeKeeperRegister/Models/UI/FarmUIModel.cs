using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.UI
{
    public class FarmUIModel
    {
        public string LocationId { get; set; } = string.Empty;
        public string? Location { get; set; }
        public int LotNo { get; set; }
        public int? NumberSpecies { get; set; }
        public int? NumberForages { get; set; }
        public int? NumberColonies { get; set; }
        public int? NumberProductioon { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
