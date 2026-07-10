using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeLocationForageService
    {
        Task<bool> AddBeeLocationForagesAsync(AddBeeLocationForageRequestModel model);
        Task<bool> DeleteBeeLocationForagesAsync(string forageCode);
        Task<List<BeeLocationForageResponseModel>?> GetAllBeeLocationForagesAsync();
        Task<List<BeeLocationForageResponseModel>?> GetAllBeeLocationForagesByLocationIdAsync(string locationId);
        Task<BeeLocationForageResponseModel?> GetBeeLocationForageByLocationIdAsync(string locationId);
        Task<int?> CountBeeLocationForagesPerFarmByLocationIdAsync(string locationId);
    }
}
