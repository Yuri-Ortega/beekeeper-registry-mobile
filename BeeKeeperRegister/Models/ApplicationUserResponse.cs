using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class ApplicationUserResponse
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? MiddleName { get; set; }
        public string? ExtensionName { get; set; }
        public string? FullName { get; set; }
        public int? SexId { get; set; }
        public string? SexDescription { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
