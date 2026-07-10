using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeProductioonService
    {
        Task<bool> AddBeeProductioonAsync(BeeProductioonResponseModel model);
        Task<bool> UpdateBeeProductioonAsync(UpdateProductioonRequestModel model);
        Task<bool> DeleteBeeProductioonByBeeProdIdAsync(string beeProdId);
        Task<List<BeeProductioonResponseModel>?> GetAllBeeProductioonAsync();
        Task<List<BeeProductioonResponseModel>?> GetAllBeeProductioonByLocationIdAsync(string locationId);
        Task<BeeProductioonResponseModel?> GetBeeProductioonByBeeProdIdAsync(string beeProdId);
        Task<int?> CountBeeProductioonPerFarmByLocationIdAsync(string locationId);
    }
}
