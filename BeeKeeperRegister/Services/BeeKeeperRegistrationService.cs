using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeKeeperRegistrationService : IBeeKeeperRegistrationService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeKeeperRegistrationService> _logger;

        private const string BaseUrl = "api/v1/BeeKeeperRegistration";

        public BeeKeeperRegistrationService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeKeeperRegistrationService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<BeeKeeperRegistrationResponseModel?> GetBeeKeeperProfileAsync()
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
                    .ReadFromJsonAsync<BeeKeeperRegistrationResponseModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching beekeeper profile");
                return null;
            }
        }

        public async Task<bool> RegisterBeeKeeperProfileAsync(AddBeeKeeperRegisterRequestModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering beekeeper");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
    }
}
