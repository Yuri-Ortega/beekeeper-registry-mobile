using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class BarangayModel
    {
        public string Bcode { get; set; } = null!;

        public string? BarangayName { get; set; }

        public string? MunCode { get; set; }
    }
}
