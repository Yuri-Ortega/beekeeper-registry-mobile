using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeKeeperTrainingService
    {
        Task<bool> AddTrainingAsync(AddBeeKeeperTrainingsModel model);
        Task<bool> UpdateTrainingAsync(UpdateBeeKeeperTrainingsModel model);
        Task<bool> DeleteTrainingByTrainingCtrAsync(int trainingCtr);
        Task<List<GetBeeKeeperTrainingsModel>?> GetAllBeeKeeperTrainingsAsync();
        Task<GetBeeKeeperTrainingModel?> GetTrainingByCtrAsync(int trainingCtr);
    }
}
