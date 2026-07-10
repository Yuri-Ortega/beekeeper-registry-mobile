using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class RegisterUserRequestModel
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string ExtensionName { get; set; } = string.Empty;
        public int SexId { get; set; }
        public string? SexDescription { get; set; }

        public DateOnly Birthday { get; set; }
    }
}
