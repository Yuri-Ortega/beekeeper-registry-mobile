using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IOneBAILookupService
    {
        Task<List<AttachmentLibraryModel>?> GetAllAttachmentLibraryAsync();
        Task<List<SexTypesModel>?> GetSexTypesAsync();
        Task<List<RegionModel>?> GetRegionsAsync();
        Task<List<ProvinceModel>?> GetProvincesByRcodeAsync(string rcode);
        Task<List<ProvinceModel>?> GetProvincesAsync();
        Task<List<CountryModel>?> GetCountriesAsync();
        Task<List<MunicipalityModel>?> GetMunicipalitiesByProvCodeAsync(string provCode);
        Task<List<BarangayModel>?> GetBarangaysByMunCodeAsync(string munCode);
    }
}
