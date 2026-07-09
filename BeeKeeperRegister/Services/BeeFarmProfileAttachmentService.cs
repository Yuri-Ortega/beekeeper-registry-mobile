using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BeeKeeperRegister.Services
{
    public class BeeFarmProfileAttachmentService : IBeeFarmProfileAttachmentService
    {
        private readonly HttpClient _httpClient;
        private readonly IDialogPopupService _popupService;
        private readonly ILogger<BeeFarmProfileAttachmentService> _logger;

        private const string BaseUrl = "api/v1/BeeFarmProfileAttachment";

        public BeeFarmProfileAttachmentService(
            IHttpClientFactory httpClientFactory,
            IDialogPopupService popupService,
            ILogger<BeeFarmProfileAttachmentService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _popupService = popupService;
            _logger = logger;
        }

        public async Task<bool> AddBeeFarmProfileAttachmentAsync(
            string locationId,
            string attachmentCode,
            string attachmentName,
            FileResult fileResult)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                using var stream = await fileResult.OpenReadAsync();
                using var streamContent = new StreamContent(stream);

                streamContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(
                        fileResult.ContentType ?? "application/octet-stream");

                var safeFileName = Path.GetFileName(fileResult.FileName);

                form.Add(streamContent, "FileDocument", safeFileName);
                form.Add(new StringContent(locationId), "LocationId");
                form.Add(new StringContent(attachmentCode), "AttachmentCode");
                form.Add(new StringContent(attachmentName), "AttachmentName");

                var response = await _httpClient.PostAsync(
                    $"{BaseUrl}", form);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Upload failed: {Error}", error);
                    _popupService.ShowInvalidDialog("Upload failed. Please try again.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading attachment");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
    }
}
