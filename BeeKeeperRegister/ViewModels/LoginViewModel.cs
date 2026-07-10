using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using BeeKeeperRegister.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BeeKeeperRegister.ViewModels
{
   public partial class LoginViewModel : ObservableObject
    {

        private readonly IAccountService _accountService;
        private readonly ILoadingPopupService _loadingPopupService;
        private readonly IDialogPopupService _popupService;

        //Login Fields
        [ObservableProperty]
        private string user = string.Empty, pass = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private bool isBusy;

        [ObservableProperty]
        private bool errUserBool, errPassBool;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private bool rememberMe;


        public LoginViewModel
        (
           IAccountService accountService,
           ILoadingPopupService loadingPopupService,
           IDialogPopupService popupService
        )
        {
            _accountService = accountService;
            _loadingPopupService = loadingPopupService;
            _popupService = popupService;
        }

        //loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var user = await SecureStorage.Default.GetAsync("rmb_user");
            var rememberMe = await SecureStorage.Default.GetAsync("rmb_ischecked");

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(rememberMe))
            {
                if (Convert.ToBoolean(rememberMe) == true)
                {
                    if (!string.IsNullOrEmpty(user))
                        User = user;

                    RememberMe = Convert.ToBoolean(rememberMe);
                }
            }
        }


        //login command
        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                ErrUserBool = string.IsNullOrWhiteSpace(User);
                ErrPassBool = string.IsNullOrWhiteSpace(Pass);
                if (ErrUserBool || ErrPassBool) return;

                using (await _loadingPopupService.Show())
                {
                    var userData = await _accountService.LoginUserAsync(new Models.Request.LoginRequestModel
                    {
                        UserName = User,
                        Password = Pass,
                    });

                    if (userData != null)
                    {
                        await SecureStorage.Default.SetAsync("access_token", userData.Token!);
                        await SecureStorage.Default.SetAsync("refresh_token", userData.RefreshToken!);
                        await SecureStorage.Default.SetAsync("access_userID", userData.UserId!.ToString());
                        await SecureStorage.Default.SetAsync("userName", userData.UserName!);


                        //Remenber me functionality
                        if (RememberMe == true)
                        {
                            await SecureStorage.Default.SetAsync("rmb_ischecked", RememberMe.ToString());
                            await SecureStorage.Default.SetAsync("rmb_user", User);
                        }
                        else if (RememberMe == false)
                        {
                            User = "";
                            SecureStorage.Default.Remove("rmb_ischecked");
                            SecureStorage.Default.Remove("rmb_user");
                        }
                        await Shell.Current.GoToAsync(nameof(DashboardUserPage));
                    }
                    else
                    {
                        _popupService.ShowInvalidDialog("Username or password entered is invalid. Please try again.");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "Server error: " + ex.Message, "OK");
            }
        }


        //navigate to create account page
        [RelayCommand]
        private async Task CreateAccountAsync()
        {
            await Shell.Current.GoToAsync(nameof(RegisterUserPage));
        }

        //Property Changed Handlers
        partial void OnUserChanged(string value)
        {
            ErrUserBool = string.IsNullOrWhiteSpace(value);
        }

        partial void OnPassChanged(string value)
        {
            ErrPassBool = string.IsNullOrEmpty(value);
        }
    }
}
