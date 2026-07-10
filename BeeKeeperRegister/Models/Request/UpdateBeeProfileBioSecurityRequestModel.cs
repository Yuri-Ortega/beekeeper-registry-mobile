using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class UpdateBeeProfileBioSecurityRequestModel
    {
        public required string LocationId { get; set; }
        public required string BeeBioCode { get; set; }
        public required bool? Result { get; set; }
    }
}
