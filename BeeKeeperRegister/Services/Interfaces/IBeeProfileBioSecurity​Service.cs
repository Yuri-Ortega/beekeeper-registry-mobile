using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
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
        Task<bool> AddBeeProfileBiosecurityAsync(AddBeeProfileBioSecurityRequestModel model);
        Task<List<BeeProfileBioSecurityResponseModel>?> GetAllBeeProfileBiosecurityByLocationIdAsync(string locationId);
        public Task<bool> UpdateBeeProfileBiosecurityAsync(UpdateBeeProfileBioSecurityRequestModel model);

        public Task<BeeProfileBioSecurityResponseModel?> GetBeeProfileBiosecurityByLocationIdAndBeeBioCodeAsync(string locationId, string beeBioCode);
    }
}
