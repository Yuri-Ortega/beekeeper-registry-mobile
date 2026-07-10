using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeLocationProductionTypeSourceService
    {
        Task<List<BeeLocationProductionTypeSourceResponseModel>?> GetAllBeeLocationProductionTypeSourcesAsync();
        Task<List<BeeLocationProductionTypeSourceResponseModel>?> GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(string locationId);
        Task<BeeLocationProductionTypeSourceResponseModel?> GetBeeLocationProductionTypeSourceByBeeProdCtrAsync(int beeProdCtr);
        Task<bool> AddBeeLocationProductionTypeSourceAsync(AddBeeLocationProductionTypeSourceRequestModel model);
        Task<bool> UpdateBeeLocationProductionTypeSourceAsync(UpdateBeeLocationProductionTypeSourceRequestModel model);
        Task<bool> DeleteBeeLocationProductionTypeSourceByBeeProdCtrAsync(int beeProdCtr);
        Task<int?> CountBeeSpeciesPerFarmByLocationIdAsync(string locationId);
        Task<int?> CountBeeColoniesPerFarmByLocationIdAsync(string locationId);
    }
}
