using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class RegisterBeeKeeperModel
    {
        public string Firstname { get; set; } = string.Empty;

        public string Lastname { get; set; } = string.Empty;

        public int ClientSexId { get; set; }
        public string ClientSexDescription { get; set; }


        public string ClientRcode { get; set; }


        public string ClientRegion { get; set; }


        public string ClientProvCode { get; set; }


        public string ClientProvinceName { get; set; }


        public string ClientMunCode { get; set; }


        public string ClientMunicipalities { get; set; }


        public string ClientBcode { get; set; }


        public string ClientBarangayName { get; set; }

        public DateOnly Birthday { get; set; }


        public string Longitude { get; set; }


        public string Latitude { get; set; }

        public DateTime? StartBeekeepingMonthYear { get; set; }

        public bool PracMigraBeekeeping { get; set; }

        public string? IfYesMigrateRemarks { get; set; }

        public int NoFarmPersonnel { get; set; }

        public bool IsHobbyist { get; set; }

        public string UserName { get; set; } = string.Empty;
    }
}
