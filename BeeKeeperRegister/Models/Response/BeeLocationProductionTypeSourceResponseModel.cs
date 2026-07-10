using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeLocationProductionTypeSourceResponseModel
    {
        public int BeeProdCtr { get; set; }
        public string ClientId { get; set; } = null!;
        public string LocationId { get; set; } = null!;
        public string BeeTypeId { get; set; } = null!;
        public string BeeTypeDescription { get; set; } = null!;
        public int? NumberColonies { get; set; }
        public string Bscolonies { get; set; } = null!;
        public string? BscoloniesDescription { get; set; }
        public string? ProvCode { get; set; }
        public string? ProvinceName { get; set; }
        public bool? IfImported { get; set; }
        public string? Gmicntry { get; set; }
        public string? CountryName { get; set; }
    }
}
