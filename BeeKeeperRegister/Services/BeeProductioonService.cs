using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeProductioonService : IBeeProductioonService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeProductioonService> _logger;

        private const string BaseUrl = "api/BeeProductioon";

        public BeeProductioonService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeProductioonService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<bool> AddBeeProductioonAsync(BeeProductioonModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bee productioon");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> UpdateBeeProductioonAsync(UpdateProductioonModel model)
        {
            try
            {
                //HttpPost → HttpPut
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}/update/{model.OldBeeProdId}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bee productioon");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> DeleteBeeProductioonByBeeProdIdAsync(string beeProdId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(
                    $"{BaseUrl}/{beeProdId}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bee productioon by beeProdId {BeeProdId}", beeProdId);
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<List<BeeProductioonModel>?> GetAllBeeProductioonAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeProductioonModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all bee productioon");
                return null;
            }
        }

        public async Task<List<BeeProductioonModel>?> GetAllBeeProductioonByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/productioons/{locationId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<BeeProductioonModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee productioon by locationId");
                return null;
            }
        }

        public async Task<BeeProductioonModel?> GetBeeProductioonByBeeProdIdAsync(string beeProdId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/productioon/{beeProdId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<BeeProductioonModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bee productioon by beeProdId");
                return null;
            }
        }

        public async Task<int?> CountBeeProductioonPerFarmByLocationIdAsync(string locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/count-productioon/{locationId}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<int?>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting bee productioon for locationId {LocationId}", locationId);
                return null;
            }
        }
    }
}
