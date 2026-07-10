using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeCommonPestResponseModel
    {
        public string CommonPests { get; set; } = null!;
        public string? CommonPestsDescription { get; set; }
    }
}
