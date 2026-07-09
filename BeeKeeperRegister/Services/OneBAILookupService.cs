using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class OneBAILookupService : IOneBAILookupService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<OneBAILookupService> _logger;

        private const string BaseUrl = "api/v1/OneBAILookup";

        public OneBAILookupService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<OneBAILookupService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<List<AttachmentLibraryModel>?> GetAllAttachmentLibraryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/all-attachment-library");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<AttachmentLibraryModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attachment library");
                return null;
            }
        }

        public async Task<List<SexTypesModel>?> GetSexTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/sex-types");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<SexTypesModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching sex types");
                return null;
            }
        }

        public async Task<List<RegionModel>?> GetRegionsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/regions");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<RegionModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching regions");
                return null;
            }
        }

        public async Task<List<ProvinceModel>?> GetProvincesByRcodeAsync(string rCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/provinces/rCode/{rCode}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<ProvinceModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching provinces by rcode");
                return null;
            }
        }

        public async Task<List<ProvinceModel>?> GetProvincesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/provinces");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<ProvinceModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching provinces");
                return null;
            }
        }

        public async Task<List<CountryModel>?> GetCountriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/countries");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<CountryModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching countries");
                return null;
            }
        }

        public async Task<List<MunicipalityModel>?> GetMunicipalitiesByProvCodeAsync(string provCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/municipalities/provCode/{provCode}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<MunicipalityModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching municipalities by provCode");
                return null;
            }
        }

        public async Task<List<BarangayModel>?> GetBarangaysByMunCodeAsync(string munCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/barangays/munCode/{munCode}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BarangayModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching barangays by munCode");
                return null;
            }
        }
    }
}
