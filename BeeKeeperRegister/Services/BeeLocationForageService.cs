using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeLocationForageService : IBeeLocationForageService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeLocationForageService> _logger;

        private const string BaseUrl = "api/BeeLocationForages";

        public BeeLocationForageService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeLocationForageService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<bool> AddBeeLocationForagesAsync(AddBeeLocationForageModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bee location forage");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> DeleteBeeLocationForagesAsync(string forageCode)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(
                    $"{BaseUrl}/location-forages/forageCode/{forageCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting forage with code {ForageCode}", forageCode);
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<List<BeeLocationForageModel>?> GetAllBeeLocationForagesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeLocationForageModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all bee location forages");
                return null;
            }
        }

        public async Task<List<BeeLocationForageModel>?> GetAllBeeLocationForagesByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/location-forages/locationId/{locationId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeLocationForageModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee location forages by locationId");
                return null;
            }
        }

        public async Task<BeeLocationForageModel?> GetBeeLocationForageByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/location-forage/locationId/{locationId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<BeeLocationForageModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee location forage by locationId");
                return null;
            }
        }

        public async Task<int?> CountBeeLocationForagesPerFarmByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/location-forages-per-farm/locationId/{locationId}/count");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<int?>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting bee location forages for locationId {LocationId}", locationId);
                return null;
            }
        }
    }
}
