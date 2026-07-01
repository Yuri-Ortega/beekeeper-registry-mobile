using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models
{
    public class GetBeeKeeperRegisterModel
    {
        public string BeeRegistrationId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public int ClientSexId { get; set; } = 0;
        public string ClientSexDescription { get; set; } = string.Empty;
        public string ClientRcode { get; set; } = string.Empty;
        public string ClientRegion { get; set; } = string.Empty;
        public string ClientProvCode { get; set; } = string.Empty;
        public string ClientProvinceName { get; set; } = string.Empty;
        public string ClientMunCode { get; set; } = string.Empty;
        public string ClientMunicipalities { get; set; } = string.Empty;
        public string ClientBcode { get; set; } = string.Empty;
        public string ClientBarangayName { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public int ClientCatId { get; set; } = 0;
        public string ClientCategory { get; set; } = string.Empty;
        public DateTime StartBeekeepingMonthYear { get; set; }
        public bool PracMigraBeekeeping { get; set; }
        public string? IfYesMigrateRemarks { get; set; }
        public int NoFarmPersonnel { get; set; }
        public bool IsHobbyist { get; set; }
        public string? UserName { get; set; }
    }
}
