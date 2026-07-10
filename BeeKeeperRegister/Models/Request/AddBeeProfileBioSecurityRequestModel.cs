using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class AddBeeProfileBioSecurityRequestModel
    {
        public required string LocationId { get; set; }
        public string BeeBioCode { get; set; } = null!;
        public string? BeeBioDescription { get; set; }
        public bool? Result { get; set; }
    }
}
