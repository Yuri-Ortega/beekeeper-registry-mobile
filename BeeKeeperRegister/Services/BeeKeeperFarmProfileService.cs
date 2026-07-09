using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeKeeperFarmProfileService : IBeeKeeperFarmProfileService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeKeeperFarmProfileService> _logger;

        private const string BaseUrl = "api/v1/BeeKeeperFarmProfileLocation";

        public BeeKeeperFarmProfileService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeKeeperFarmProfileService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<bool> AddFarmProfileAsync(AddBeeKeeperFarmProfileLocationModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}", model);

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Failed to add farm profile.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding farm profile");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> UpdateFarmProfileAsync(UpdateBeeKeeperFarmProfileLocationModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(
                    $"{BaseUrl}/{model.LocationId}", model);

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Failed to update farm profile.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating farm profile");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<BeeKeeperFarmProfileLocationModel?> GetFarmProfileByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{locationId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<BeeKeeperFarmProfileLocationModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching farm profile by locationId");
                return null;
            }
        }

        public async Task<List<BeeKeeperFarmProfileLocationModel>?> GetAllFarmProfilesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeKeeperFarmProfileLocationModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all farm profiles");
                return null;
            }
        }

        public async Task<int?> CountFarmProfilesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/count-farms");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<int?>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting farm profiles");
                return null;
            }
        }
    }
}
