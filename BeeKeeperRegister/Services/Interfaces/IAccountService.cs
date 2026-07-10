using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool IsSuccess, string Message)> UploadValidIdAndSignatureAsync(
            //FileResult? profilePicture,
            FileResult? validId = null,
            FileResult? signature = null
        );

        public Task<(bool IsSuccess, string Message)> UploadProfileImageAsync(
byte[] profilePictureBytes, string fileName = "profile.png", string contentType = "image/png");

        public Task<UserImageResponseModel?> GetProfileImageAsync();
        public Task<ApplicationUserResponseModel?> GetUserProfileAsync();
        public Task<LoginResponseModel?> LoginUserAsync(LoginRequestModel model);
        public Task<bool?> RegisterUserAsync(RegisterUserRequestModel model);
    }
}
