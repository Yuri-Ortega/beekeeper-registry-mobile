using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BarangayResponseModel
    {
        public string Bcode { get; set; } = null!;

        public string? BarangayName { get; set; }

        public string? MunCode { get; set; }
    }
}
