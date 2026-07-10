using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class AddBeeLocationProductionTypeSourceRequestModel
    {
        public required string LocationId { get; set; } = null!;

        public required string BeeTypeId { get; set; } = null!;

        public required string BeeTypeDescription { get; set; } = null!;

        public required int NumberColonies { get; set; }

        public required string Bscolonies { get; set; } = null!;

        public required string BscoloniesDescription { get; set; }

        public string? ProvCode { get; set; }

        public string? ProvinceName { get; set; }

        public bool? IfImported { get; set; }
        public string? Gmicntry { get; set; }
        public string? CountryName { get; set; }
    }
}
