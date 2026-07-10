using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class SexTypesResponseModel
    {
        public int SexId { get; set; }
        public string? SexDescription { get; set; }
    }
}
