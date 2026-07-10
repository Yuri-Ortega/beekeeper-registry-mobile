using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class AddBeeKeeperFarmProfileLocationRequestModel
    {
        public required string Rcode { get; set; }


        public required string Regions { get; set; }


        public required string ProvCode { get; set; }


        public required string Provinces { get; set; }


        public required string MunCode { get; set; }


        public required string Municipalities { get; set; }

        public required int LotNo { get; set; }


        public required string Bcode { get; set; }


        public required string Barangay { get; set; }


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
