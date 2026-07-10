using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class UpdateBeeKeeperFarmProfileLocationRequestModel
    {
       public required string LocationId { get; set; }

        public required string Latitude { get; set; }

        public required string Longitude { get; set; }
        public bool Hdmng { get; set; }

        public string? BeeProSysId { get; set; }

        public string? BeeSystemProduction { get; set; }

        public string? CommonDiseaseBee { get; set; }

        public string? CommonDiseaseBeeDescription { get; set; }

        public string? CommonPests { get; set; }

        public string? CommonPestsDescription { get; set; }
    }
}
