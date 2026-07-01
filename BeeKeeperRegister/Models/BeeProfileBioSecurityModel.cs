using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class BeeProfileBioSecurityModel
    {
        public string LocationId { get; set; }
        public string ClientId { get; set; }
        public string BeeBioCode { get; set; }
        public string? BeeBioDescription { get; set; }
        public bool? Result { get; set; }
        public string? Remarks { get; set; }
    }
}
