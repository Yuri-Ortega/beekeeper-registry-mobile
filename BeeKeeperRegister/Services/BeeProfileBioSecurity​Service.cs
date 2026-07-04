using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeProfileBioSecurityService : IBeeProfileBioSecurityService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeProfileBioSecurityService> _logger;

        private const string BaseUrl = "api/BeeProfileBioSecurity";

        public BeeProfileBioSecurityService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeProfileBioSecurityService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<bool> AddBeeProfileBiosecurityAsync(AddBeeProfileBioSecurityModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bee profile biosecurity");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> UpdateBeeProfileBiosecurityAsync(UpdateBeeProfileBioSecurityModel model)
        {
            try
            {
                var response = await _httpClient.PatchAsJsonAsync(
                    $"{BaseUrl}/{model.LocationId}/{model.BeeBioCode}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating biosecurity");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<List<BeeProfileBioSecurityModel>?> GetAllBeeProfileBiosecurityByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{locationId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<List<BeeProfileBioSecurityModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee profile biosecurity by locationId");
                return null;
            }
        }

        public async Task<BeeProfileBioSecurityModel?> GetBeeProfileBiosecurityByLocationIdAndBeeBioCodeAsync(string locationId, string beeBioCode)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{locationId}/{beeBioCode}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<BeeProfileBioSecurityModel?>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee profile biosecurity by locationId and beeBioCode");
                return null;
            }
        }


    }
}
