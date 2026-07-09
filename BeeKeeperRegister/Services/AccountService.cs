using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services
{
    public class AccountService : IAccountService
    {


        private readonly HttpClient _httpClient;
        private readonly ILogger<AccountService> _logger;

        private const string BaseUrl = "api/v1/Account";

        public AccountService(IHttpClientFactory httpClientFactory,ILogger<AccountService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _logger = logger;
        }


        public async Task<(bool IsSuccess, string Message)> UploadValidIdAndSignatureAsync(
            //FileResult? profilePicture,
            FileResult? validId = null,
            FileResult? signature = null)
        {
            const long maxFileSize = 5 * 1024 * 1024;

            using var content = new MultipartFormDataContent();

            if (validId is not null)
            {
                var stream = await validId.OpenReadAsync();
                if (stream.Length > maxFileSize)
                    return (false, "Valid ID exceeds 5MB limit.");

                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(validId.ContentType);
                content.Add(streamContent, "UploadValidId", validId.FileName);
            }

            if (signature is not null)
            {
                var stream = await signature.OpenReadAsync();
                if (stream.Length > maxFileSize)
                    return (false, "Signature exceeds 5MB limit.");

                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(signature.ContentType);
                content.Add(streamContent, "Signature", signature.FileName);
            }

            //if (profilePicture is not null)
            //{
            //    var stream = await profilePicture.OpenReadAsync();
            //    if (stream.Length > maxFileSize)
            //        return (false, "Profile picture exceeds 5MB limit.");

            //    var streamContent = new StreamContent(stream);
            //    streamContent.Headers.ContentType = new MediaTypeHeaderValue(profilePicture.ContentType);
            //    content.Add(streamContent, "ProfilePicture", profilePicture.FileName);
            //}

            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/uploadProfileImage", content);

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    return (true, message);
                }

                var error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, $"Upload failed: {ex.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> UploadProfileImageAsync(
        byte[] profilePictureBytes, string fileName = "profile.png", string contentType = "image/png")
        {
            const long maxFileSize = 5 * 1024 * 1024;

            if (profilePictureBytes.Length > maxFileSize)
                return (false, "Profile picture exceeds 5MB limit.");

            using var content = new MultipartFormDataContent();
            using var stream = new MemoryStream(profilePictureBytes);
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            content.Add(streamContent, "ProfilePicture", fileName);  

            try
            {
                var response = await _httpClient.PostAsync($"{BaseUrl}/uploadProfileImage", content);

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    return (true, message);
                }

                var error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, $"Upload failed: {ex.Message}");
            }
        }

        public async Task<UserImageResponseModel?> GetProfileImageAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/ProfileImages");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<UserImageResponseModel>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching Images profile");
                return null;
            }
        }


        public async Task<ApplicationUserResponse?> GetUserProfileAsync()
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
                    .ReadFromJsonAsync<ApplicationUserResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user profile");
                return null;
            }
        }



        public async Task<LoginResponseModel?> LoginUserAsync(LoginRequestModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login-user", model);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<LoginResponseModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error login request");
                return null;
            }
        }


        public async Task<bool?> RegisterUserAsync(RegisterUserModel model)
        {
            try {

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/register-user", model);


                return response.IsSuccessStatusCode ? true : false;
            } catch (Exception ex)
            {
                _logger.LogError(ex, "Error register user");
                return false;
            }
        }
    }
}
