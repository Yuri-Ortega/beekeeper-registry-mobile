using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class UpdateBeeKeeperTrainingsModel
    {
        public int? TrainingCtr { get; set; }
        public string? TrainingCode { get; set; }
        public string? TrainingDescription { get; set; }
        public int? TrainingYear { get; set; }
        public int? NumberofHrs { get; set; }
    }
}
