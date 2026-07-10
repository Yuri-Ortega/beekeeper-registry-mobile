using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class BeeProfileBioSecurityResponseModel
    {
        public string LocationId { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string BeeBioCode { get; set; } = null!;
        public string? BeeBioDescription { get; set; }
        public bool? Result { get; set; }
        public string? Remarks { get; set; }
    }
}
