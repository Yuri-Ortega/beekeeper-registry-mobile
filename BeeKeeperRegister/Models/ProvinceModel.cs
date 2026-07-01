using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class ProvinceModel
    {
        public string ProvCode { get; set; } = null!;
        public string? ProvinceName { get; set; }
        public string? Rcode { get; set; }
    }
}