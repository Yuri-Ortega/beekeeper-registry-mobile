using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeKeeperRegistrationService
    {
        Task<BeeKeeperRegistrationResponseModel?> GetBeeKeeperProfileAsync();
        Task<bool> RegisterBeeKeeperProfileAsync(AddBeeKeeperRegisterRequestModel model);
    }
}
