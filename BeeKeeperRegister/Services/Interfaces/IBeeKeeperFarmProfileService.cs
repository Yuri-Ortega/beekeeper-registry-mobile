using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeKeeperFarmProfileService
    {
        Task<bool> AddFarmProfileAsync(AddBeeKeeperFarmProfileLocationModel model);
        Task<bool> UpdateFarmProfileAsync(UpdateBeeKeeperFarmProfileLocationModel model);
        Task<BeeKeeperFarmProfileLocationModel?> GetFarmProfileByLocationIdAsync(string locationId);
        Task<List<BeeKeeperFarmProfileLocationModel>?> GetAllFarmProfilesAsync();
        Task<int?> CountFarmProfilesAsync();
    }
}
