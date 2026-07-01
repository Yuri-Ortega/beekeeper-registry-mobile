using BeeKeeperRegister.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace BeeKeeperRegister.Views;

public partial class ViewFarmProfilePage : ContentPage
{
    public ViewFarmProfilePage(ViewFarmProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewFarmProfileViewModel vm)
        {
            vm.MoveMapAction = (location) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    MyMap.MoveToRegion(
                        MapSpan.FromCenterAndRadius(location, Distance.FromMeters(600)));

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