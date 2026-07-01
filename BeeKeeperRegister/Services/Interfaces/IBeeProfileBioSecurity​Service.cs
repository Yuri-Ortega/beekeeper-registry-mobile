using BeeKeeperRegister.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeProfileBioSecurity​Service
    {
        Task<bool> AddBeeProfileBiosecurityAsync(AddBeeProfileBioSecurityModel model);
        Task<List<BeeProfileBioSecurityModel>?> GetAllBeeProfileBiosecurityByLocationIdAsync(string locationId);
        public Task<bool> UpdateBeeProfileBiosecurityAsync(UpdateBeeProfileBioSecurityModel model);

        public Task<BeeProfileBioSecurityModel?> GetBeeProfileBiosecurityByLocationIdAndBeeBioCodeAsync(string locationId, string beeBioCode);
    }
}
