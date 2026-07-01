using BeeKeeperRegister.Components.Classes;
using DevExpress.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace BeeKeeperRegister.Components.Views;

public partial class UsableGoogleMapDialogPopup : DXPopup
{
    public UsableGoogleMapDialogPopup()
    {
        InitializeComponent();
    }

    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        var location = e.Location;

        double lat = location.Latitude;
        double lng = location.Longitude;

        if (lat < 4.5 || lat > 21.0 || lng < 116.0 || lng > 127.0)
            return;

        MyMap.Pins.Clear();

        var pin = new Pin
        {
            Label = "Selected Location",
            Location = new Location(lat, lng)
        };

        MyMap.Pins.Add(pin);

        MyMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(location, Distance.FromMeters(500)));

        if (BindingContext is GoogleMapPopupViewModel vm)
        {
            vm.UpdateLocation(lat, lng);
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is GoogleMapPopupViewModel vm)
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
        }
    }
}