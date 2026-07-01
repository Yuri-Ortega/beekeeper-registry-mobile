using BeeKeeperRegister.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace BeeKeeperRegister.Views;

public partial class UpdateFarmProfilePage : ContentPage
{
    public UpdateFarmProfilePage(UpdateFarmProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UpdateFarmProfileViewModel vm)
        {
            vm.MoveMapAction = (location) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MyMap.MoveToRegion(
                        MapSpan.FromCenterAndRadius(location, Distance.FromMeters(500)));

                    MyMap.Pins.Clear();

                    MyMap.Pins.Add(new Pin
                    {
                        Label = "Auto Location",
                        Location = location
                    });
                });
            };
            await vm.LoaderAsync();
        }
        
    }
}