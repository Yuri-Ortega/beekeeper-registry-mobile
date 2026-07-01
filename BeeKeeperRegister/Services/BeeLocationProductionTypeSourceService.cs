using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeLocationProductionTypeSourceService : IBeeLocationProductionTypeSourceService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeLocationProductionTypeSourceService> _logger;

        private const string BaseUrl = "api/BeeLocationProductionTypeSource";

        public BeeLocationProductionTypeSourceService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeLocationProductionTypeSourceService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<List<BeeLocationProductionTypeSourceModel>?> GetAllBeeLocationProductionTypeSourcesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<List<BeeLocationProductionTypeSourceModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all production type sources");
                return null;
            }
        }

        public async Task<List<BeeLocationProductionTypeSourceModel>?> GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/location-production-type-source/locationId/{locationId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<List<BeeLocationProductionTypeSourceModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching production type sources by locationId {LocationId}", locationId);
                return null;
            }
        }

        public async Task<BeeLocationProductionTypeSourceModel?> GetBeeLocationProductionTypeSourceByBeeProdCtrAsync(int beeProdCtr)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/location-production-type-source/beeProdCtr/{beeProdCtr}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<BeeLocationProductionTypeSourceModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching production type source by beeProdCtr {BeeProdCtr}", beeProdCtr);
                return null;
            }
        }

        public async Task<bool> AddBeeLocationProductionTypeSourceAsync(AddBeeLocationProductionTypeSourceModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding production type source");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> UpdateBeeLocationProductionTypeSourceAsync(UpdateBeeLocationProductionTypeSourceModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(
                    $"{BaseUrl}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating production type source");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> DeleteBeeLocationProductionTypeSourceByBeeProdCtrAsync(int beeProdCtr)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(
                    $"{BaseUrl}/location-production-type-source/beeProdCtr/{beeProdCtr}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting production type source");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<int?> CountBeeSpeciesPerFarmByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/species-per-farm/locationId/{locationId}/count");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<int?>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting bee species for locationId {LocationId}", locationId);
                return null;
            }
        }

        public async Task<int?> CountBeeColoniesPerFarmByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/colonies-per-farm/locationId/{locationId}/count");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<int?>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting bee colonies for locationId {LocationId}", locationId);
                return null;
            }
        }
    }
}
