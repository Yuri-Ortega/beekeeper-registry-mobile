using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class RefreshTokenRequestModel
    {
        public required string Token { get; set; } = string.Empty;
        public required string RefreshToken { get; set; } = string.Empty;
    }
}
