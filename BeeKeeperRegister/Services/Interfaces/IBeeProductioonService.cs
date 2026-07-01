using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeProductioonService
    {
        Task<bool> AddBeeProductioonAsync(BeeProductioonModel model);
        Task<bool> UpdateBeeProductioonAsync(UpdateProductioonModel model);
        Task<bool> DeleteBeeProductioonByBeeProdIdAsync(string beeProdId);
        Task<List<BeeProductioonModel>?> GetAllBeeProductioonAsync();
        Task<List<BeeProductioonModel>?> GetAllBeeProductioonByLocationIdAsync(string locationId);
        Task<BeeProductioonModel?> GetBeeProductioonByBeeProdIdAsync(string beeProdId);
        Task<int?> CountBeeProductioonPerFarmByLocationIdAsync(string locationId);
    }
}
