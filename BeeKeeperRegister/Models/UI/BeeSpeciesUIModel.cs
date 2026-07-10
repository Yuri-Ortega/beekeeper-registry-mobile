using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.UI
{
    public class BeeSpeciesUIModel
    {
        public int Id { get; set; }
        public string? BeeTypeDescription { get; set; }
        public string? Origin { get; set; }
        public int? NOC { get; set; }
        public string? Source { get; set; }
        public string? Imported { get; set; }
    }
}