using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class SexTypesModel
    {
        public int SexId { get; set; }
        public string? SexDescription { get; set; }
    }
}
