using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
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

        private const string BaseUrl = "api/AHWDLookup";

        public AHWDLookupService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<AHWDLookupService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<List<BeeProductionSystemModel>?> GetAllBeeProductionSystemsAsync()
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

                return await response.Content.ReadFromJsonAsync<List<BeeProductionSystemModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee production systems");
                return null;
            }
        }

        public async Task<List<BeeCommonPestModel>?> GetAllBeeCommonPestsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-common-pests");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeCommonPestModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee common pests");
                return null;
            }
        }

        public async Task<List<BeeCommonDiseasesModel>?> GetAllBeeCommonDiseasesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-common-diseases");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeCommonDiseasesModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee common diseases");
                return null;
            }
        }


        public async Task<List<BeeTrainingModel>?> GetAllBeeTrainingsAsync()
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
                    .ReadFromJsonAsync<List<BeeTrainingModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee trainings");
                return null;
            }
        }


        public async Task<List<BeeForagesModel>?> GetAllBeeForagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-forages");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeForagesModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee forages");
                return null;
            }
        }


        public async Task<List<BeeTypesModel>?> GetAllBeeTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-types");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeTypesModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee types");
                return null;
            }
        }

        public async Task<List<BeeSourceColoniesModel>?> GetAllBeeSourcesColoniesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-sources-colonies");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeSourceColoniesModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee sources colonies");
                return null;
            }
        }


        public async Task<List<BeeProductionModel>?> GetAllBeeProductionCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-production-categories");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeProductionModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee production categories");
                return null;
            }
        }


        public async Task<List<BeeBioSecurityModel>?> GetAllBeeBiosecuritiesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/bee-biosecurities");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeBioSecurityModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee biosecurities");
                return null;
            }
        }
    }
}
