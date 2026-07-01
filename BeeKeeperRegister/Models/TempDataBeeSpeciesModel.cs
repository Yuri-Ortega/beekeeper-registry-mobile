using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class TempDataBeeSpeciesModel
    {
        public int Id { get; set; }
        public string LocationId { get; set; }
        public string BeeTypeId { get; set; }
        public string BeeTypeDescription { get; set; }
        public int NumberColonies { get; set; }

        public string Bscolonies { get; set; }

        public string BscoloniesDescription { get; set; }

        public string? ProvCode { get; set; }

        public string? ProvinceName { get; set; }

        public bool? IfImported { get; set; }
        public string? Gmicntry { get; set; }
        public string? CountryName { get; set; }
    }
}
