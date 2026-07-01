using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeLocationForageService
    {
        Task<bool> AddBeeLocationForagesAsync(AddBeeLocationForageModel model);
        Task<bool> DeleteBeeLocationForagesAsync(string forageCode);
        Task<List<BeeLocationForageModel>?> GetAllBeeLocationForagesAsync();
        Task<List<BeeLocationForageModel>?> GetAllBeeLocationForagesByLocationIdAsync(string locationId);
        Task<BeeLocationForageModel?> GetBeeLocationForageByLocationIdAsync(string locationId);
        Task<int?> CountBeeLocationForagesPerFarmByLocationIdAsync(string locationId);
    }
}
