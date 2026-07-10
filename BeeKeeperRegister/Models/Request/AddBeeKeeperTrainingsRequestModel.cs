using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class AddBeeKeeperTrainingsRequestModel
    {
        public required string TrainingCode { get; set; }

        public required string TrainingDescription { get; set; }

        public required int TrainingYear { get; set; }

        public required int NumberofHrs { get; set; }
    }
}
