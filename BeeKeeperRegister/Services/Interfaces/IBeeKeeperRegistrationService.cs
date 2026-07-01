using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeKeeperRegistrationService
    {
        Task<GetBeeKeeperRegisterModel?> GetBeeKeeperProfileAsync();
        Task<bool> RegisterBeeKeeperProfileAsync(RegisterBeeKeeperModel model);
    }
}
