using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Handler
{
    public class LogoutHandler
    {
        public async Task LogoutClearDataAsync()
        {
            SecureStorage.Remove("access_token");
            SecureStorage.Remove("refresh_token");
            SecureStorage.Remove("access_userID");
            SecureStorage.Remove("userName");

            await Task.CompletedTask;
        }
    }
}
