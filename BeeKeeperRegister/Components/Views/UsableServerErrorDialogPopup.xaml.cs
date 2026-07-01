using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Views;
using DevExpress.Maui.Controls;
using Microsoft.Maui.Handlers;

namespace BeeKeeperRegister.Components.Views;

public partial class UsableServerErrorDialogPopup : DXPopup
{
    private readonly LogoutHandler _logoutHandler;
    public UsableServerErrorDialogPopup(LogoutHandler logoutHandler)
    {
        InitializeComponent();
        _logoutHandler = logoutHandler;
    }

    private async void TryAgainBTN(object sender, EventArgs e)
    {
        Popup.IsOpen = false;
        await Shell.Current.GoToAsync(nameof(LoginPage));
        await _logoutHandler.LogoutClearDataAsync();
    }
}