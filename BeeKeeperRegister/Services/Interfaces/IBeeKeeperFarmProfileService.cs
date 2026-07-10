using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeKeeperFarmProfileService
    {
        Task<bool> AddFarmProfileAsync(AddBeeKeeperFarmProfileLocationRequestModel model);
        Task<bool> UpdateFarmProfileAsync(UpdateBeeKeeperFarmProfileLocationRequestModel model);
        Task<BeeKeeperFarmProfileLocationResponseModel?> GetFarmProfileByLocationIdAsync(string locationId);
        Task<List<BeeKeeperFarmProfileLocationResponseModel>?> GetAllFarmProfilesAsync();
        Task<int?> CountFarmProfilesAsync();
    }
}
