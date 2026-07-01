using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class AddBeeKeeperFarmProfileLocationModel
    {
        public string Rcode { get; set; }


        public string Regions { get; set; }


        public string ProvCode { get; set; }


        public string Provinces { get; set; }


        public string MunCode { get; set; }


        public string Municipalities { get; set; }

        public int LotNo { get; set; }


        public string Bcode { get; set; }


        public string Barangay { get; set; }


        public string Latitude { get; set; }


        public string Longitude { get; set; }

        public bool Hdmng { get; set; }

        public string BeeProSysId { get; set; }

        public string BeeSystemProduction { get; set; }

        public string CommonDiseaseBee { get; set; }

        public string CommonDiseaseBeeDescription { get; set; }

        public string CommonPests { get; set; }

        public string CommonPestsDescription { get; set; }
    }
}
