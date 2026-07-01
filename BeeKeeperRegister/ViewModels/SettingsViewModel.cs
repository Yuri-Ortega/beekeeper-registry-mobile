using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BeeKeeperRegister.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly LogoutHandler _logoutHandler;

        public SettingsViewModel(LogoutHandler logoutHandler)
            => _logoutHandler = logoutHandler;

        //logout command
        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _logoutHandler.LogoutClearDataAsync();
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}
