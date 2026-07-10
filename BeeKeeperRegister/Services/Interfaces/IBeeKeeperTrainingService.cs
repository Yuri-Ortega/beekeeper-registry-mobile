using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeKeeperTrainingService
    {
        Task<bool> AddTrainingAsync(AddBeeKeeperTrainingsRequestModel model);
        Task<bool> UpdateTrainingAsync(UpdateBeeKeeperTrainingsRequestModel model);
        Task<bool> DeleteTrainingByTrainingCtrAsync(int trainingCtr);
        Task<List<AllBeeKeeperTrainingsResponseModel>?> GetAllBeeKeeperTrainingsAsync();
        Task<BeeKeeperTrainingResponseModel?> GetTrainingByCtrAsync(int trainingCtr);
    }
}
