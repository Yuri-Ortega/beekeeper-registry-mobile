using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class CountryModel
    {
        public double? Cid { get; set; }
        public string? CountryName { get; set; }
        public string? Gmicntry { get; set; } = null!;
        public string? Iso2 { get; set; }
        public string? Capital { get; set; }
        public string? Currency { get; set; }
        public string? Nationality { get; set; }
    }
}
