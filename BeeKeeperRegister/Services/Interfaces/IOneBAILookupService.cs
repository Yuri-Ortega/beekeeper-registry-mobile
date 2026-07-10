using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IOneBAILookupService
    {
        Task<List<AttachmentLibraryResponseModel>?> GetAllAttachmentLibraryAsync();
        Task<List<SexTypesResponseModel>?> GetSexTypesAsync();
        Task<List<RegionResponseModel>?> GetRegionsAsync();
        Task<List<ProvinceResponseModel>?> GetProvincesByRcodeAsync(string rcode);
        Task<List<ProvinceResponseModel>?> GetProvincesAsync();
        Task<List<CountryResponseModel>?> GetCountriesAsync();
        Task<List<MunicipalityResponseModel>?> GetMunicipalitiesByProvCodeAsync(string provCode);
        Task<List<BarangayResponseModel>?> GetBarangaysByMunCodeAsync(string munCode);
    }
}
