using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeTypesResponseModel
    {
        public string BeeTypeId { get; set; } = null!;
        public string BeeTypeDescription { get; set; } = null!;
    }
}
