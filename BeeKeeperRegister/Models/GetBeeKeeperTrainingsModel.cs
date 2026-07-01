using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class GetBeeKeeperTrainingsModel
    {
        public string ClientId { get; set; } = null!;
        public int TrainingCtr { get; set; }
        public string? TrainingCode { get; set; }
        public string? TrainingDescription { get; set; }
        public int? TrainingYear { get; set; }
        public int? NumberofHrs { get; set; }
    }
}
