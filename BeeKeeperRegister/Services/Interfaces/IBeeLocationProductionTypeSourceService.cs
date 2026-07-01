using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeLocationProductionTypeSourceService
    {
        Task<List<BeeLocationProductionTypeSourceModel>?> GetAllBeeLocationProductionTypeSourcesAsync();
        Task<List<BeeLocationProductionTypeSourceModel>?> GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(string locationId);
        Task<BeeLocationProductionTypeSourceModel?> GetBeeLocationProductionTypeSourceByBeeProdCtrAsync(int beeProdCtr);
        Task<bool> AddBeeLocationProductionTypeSourceAsync(AddBeeLocationProductionTypeSourceModel model);
        Task<bool> UpdateBeeLocationProductionTypeSourceAsync(UpdateBeeLocationProductionTypeSourceModel model);
        Task<bool> DeleteBeeLocationProductionTypeSourceByBeeProdCtrAsync(int beeProdCtr);
        Task<int?> CountBeeSpeciesPerFarmByLocationIdAsync(string locationId);
        Task<int?> CountBeeColoniesPerFarmByLocationIdAsync(string locationId);
    }
}
