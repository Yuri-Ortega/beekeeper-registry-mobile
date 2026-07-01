using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class UpdateBeeProfileBioSecurityModel
    {
        public string LocationId { get; set; }
        public string BeeBioCode { get; set; }
        public bool? Result { get; set; }
    }
}
