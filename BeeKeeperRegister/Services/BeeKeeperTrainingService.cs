using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeKeeperTrainingService : IBeeKeeperTrainingService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeKeeperTrainingService> _logger;

        private const string BaseUrl = "api/v1/BeeKeeperTrainings";

        public BeeKeeperTrainingService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeKeeperTrainingService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<bool> AddTrainingAsync(AddBeeKeeperTrainingsModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"{BaseUrl}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding training");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> UpdateTrainingAsync(UpdateBeeKeeperTrainingsModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(
                    $"{BaseUrl}/{model.TrainingCtr}", model);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating training");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<bool> DeleteTrainingByTrainingCtrAsync(int trainingCtr)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(
                    $"{BaseUrl}/{trainingCtr}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting training");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }

        public async Task<List<GetBeeKeeperTrainingsModel>?> GetAllBeeKeeperTrainingsAsync()
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
                    .ReadFromJsonAsync<List<GetBeeKeeperTrainingsModel>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all trainings");
                return null;
            }
        }

        public async Task<GetBeeKeeperTrainingModel?> GetTrainingByCtrAsync(int trainingCtr)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{trainingCtr}");

                if (!response.IsSuccessStatusCode)
                {
                    //_popupService.ShowServerErrorDialog("Server is temporarily unavailable.");
                    return null;
                }

                return await response.Content
                    .ReadFromJsonAsync<GetBeeKeeperTrainingModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching training by ctr");
                return null;
            }
        }
    }
}
