using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Request
{
    public class AddBeeKeeperRegisterRequestModel
    {
        public required string Firstname { get; set; } = string.Empty;

        public required string Lastname { get; set; } = string.Empty;

        public required int ClientSexId { get; set; }


        public required string ClientSexDescription { get; set; }


        public required string ClientRcode { get; set; }


        public required string ClientRegion { get; set; }


        public required string ClientProvCode { get; set; }


        public required string ClientProvinceName { get; set; }


        public required string ClientMunCode { get; set; }


        public required string ClientMunicipalities { get; set; }


        public required string ClientBcode { get; set; }


        public required string ClientBarangayName { get; set; }

        public required DateOnly Birthday { get; set; }


        public required string Longitude { get; set; }


        public required string Latitude { get; set; }

        public required DateTime StartBeekeepingMonthYear { get; set; }

        public bool PracMigraBeekeeping { get; set; }

        public string? IfYesMigrateRemarks { get; set; }

        public required int NoFarmPersonnel { get; set; }

        public bool IsHobbyist { get; set; }

        public required string UserName { get; set; }
    }
}
