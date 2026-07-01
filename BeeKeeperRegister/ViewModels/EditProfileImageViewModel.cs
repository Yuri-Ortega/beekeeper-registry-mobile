using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.ViewModels
{
    public partial class EditProfileImageViewModel : ObservableObject
    {
        private readonly IAccountService _accountService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;

        [ObservableProperty]
        private ImageSource selectedImageSource;

        public ImageEdit ImageEditControl { get; set; }

        private byte[]? _editedImageBytes;
        private byte[]? _originalImageBytes;

        private ImageSource? _cachedUsers;

        private FileResult? _profilePictureFile;

        public EditProfileImageViewModel(IAccountService accountService, ILoadingPopupService loading, IDialogPopupService popupService)
        {
            _accountService = accountService;
            _loading = loading;
            _popupService = popupService;
        }

        // Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var result = await _accountService.GetProfileImageAsync();

            if (result is not null && result?.ProfilePicture is not null && result.ProfilePicture.Length > 0)
                SelectedImageSource = ImageSource.FromStream(
                        () => new MemoryStream(result.ProfilePicture));
            else
                SelectedImageSource = ImageSource.FromFile("default_user.jpg");
        }

        [RelayCommand]
        private async Task PickImgBTNAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();

                if (result == null)
                    return;

                var stream = await result.OpenReadAsync();
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                //memoryStream.Position = 0;
                _originalImageBytes = memoryStream.ToArray();
                SelectedImageSource = ImageSource.FromStream(() => new MemoryStream(_originalImageBytes));
                _editedImageBytes = null;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task SaveImgBTNAsync()
        {
            try
            {
                if (SelectedImageSource is FileImageSource fileSource &&
                            string.Equals(fileSource.File, "default_user.jpg", StringComparison.OrdinalIgnoreCase))
                {
                    _popupService.ShowInvalidDialog("Invalid default image. Please pick image first");
                    return;
                }

                var confirmed = await _popupService.ShowConfirmDialog("Save Photo","Do you want to save changes?", "Yes", "No");
                if (!confirmed) return;

                using (await _loading.Show())
                {
                    using var memoryStream = new MemoryStream();
                    ImageEditControl.SaveAsStream(memoryStream, DevExpress.Maui.Editors.ImageFormat.Png, true);

                    _editedImageBytes = memoryStream.ToArray();

                    if (_editedImageBytes is null || _editedImageBytes.Length == 0)
                    {
                        _popupService.ShowInvalidDialog("Failed to process the image. Please try again.");
                        return;
                    }

                    SelectedImageSource = ImageSource.FromStream(() => new MemoryStream(_editedImageBytes));

                    var (isSuccess, message) = await _accountService.UploadProfileImageAsync(
                        _editedImageBytes, "profile.png", "image/png");

                    if (isSuccess)
                        _popupService.ShowSuccessDialog(message);
                    else
                        _popupService.ShowInvalidDialog(message);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
