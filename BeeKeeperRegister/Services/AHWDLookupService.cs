using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models.Response;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services
{
    public class AHWDLookupService : IAHWDLookupService
    {

        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<AHWDLookupService> _logger;

        private const string BaseUrl = "api/v1/AHWDLookup";

        public AHWDLookupService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<AHWDLookupService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<List<BeeProductionSystemResponseModel>?> GetAllBeeProductionSystemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{BaseUrl}/bee-production-systems");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeProductionSystemResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee production systems");
                return null;
            }
        }

        public async Task<List<BeeCommonPestResponseModel>?> GetAllBeeCommonPestsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-common-pests");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeCommonPestResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee common pests");
                return null;
            }
        }

        public async Task<List<BeeCommonDiseasesResponseModel>?> GetAllBeeCommonDiseasesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-common-diseases");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeCommonDiseasesResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee common diseases");
                return null;
            }
        }


        public async Task<List<BeeTrainingResponseModel>?> GetAllBeeTrainingsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-trainings");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<List<BeeTrainingResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee trainings");
                return null;
            }
        }


        public async Task<List<BeeForagesResponseModel>?> GetAllBeeForagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-forages");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeForagesResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee forages");
                return null;
            }
        }


        public async Task<List<BeeTypesResponseModel>?> GetAllBeeTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-types");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeTypesResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee types");
                return null;
            }
        }

        public async Task<List<BeeSourceColoniesResponseModel>?> GetAllBeeSourcesColoniesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-sources-colonies");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeSourceColoniesResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee sources colonies");
                return null;
            }
        }


        public async Task<List<BeeProductionResponseModel>?> GetAllBeeProductionCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-production-categories");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeProductionResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee production categories");
                return null;
            }
        }


        public async Task<List<BeeBioSecurityResponseModel>?> GetAllBeeBiosecuritiesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-biosecurities");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeBioSecurityResponseModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee biosecurities");
                return null;
            }
        }
    }
}
