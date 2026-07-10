using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class MunicipalityResponseModel
    {
        public string MunCode { get; set; } = null!;

        public string? MunCity { get; set; }

        public string? ProvCode { get; set; }
    }
}
