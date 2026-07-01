using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}