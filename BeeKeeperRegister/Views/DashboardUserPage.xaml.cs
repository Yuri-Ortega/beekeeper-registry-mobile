using BeeKeeperRegister.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace BeeKeeperRegister.Views;

public partial class DashboardUserPage : ContentPage
{
	public DashboardUserPage(DashboardUserViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DashboardUserViewModel vm)
        {
            await vm.LoaderAsync();
        }

    }
}