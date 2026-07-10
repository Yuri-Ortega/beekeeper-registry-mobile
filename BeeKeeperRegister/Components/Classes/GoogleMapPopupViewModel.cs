using BeeKeeperRegister.Models.Response;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Components.Classes
{
    public partial class GoogleMapPopupViewModel : ObservableObject
    {
        private TaskCompletionSource<GoogleMapResponseModel> _tcs;

        [ObservableProperty]
        private double latitude;
        [ObservableProperty]
        private double longitude;

        public Command ConfirmCommand { get; }

        public GoogleMapPopupViewModel(string region, string province, string municipality, string barangay, double? latitude = null, double? longitude = null)
        {
            _tcs = new TaskCompletionSource<GoogleMapResponseModel>();

            ConfirmCommand = new Command(OnClose);

            _ = InitializeLocation(region, province, municipality, barangay, latitude, longitude);
        }

        private async Task InitializeLocation(string region, string province, string municipality, string barangay, double? lat, double? lng)
        {

            if (string.IsNullOrEmpty(barangay)) return;

            var address = $"{barangay}, {municipality}, {province}, {region}, Philippines";

            var locations = await Geocoding.GetLocationsAsync(address);
            var location = locations?.FirstOrDefault();

            if (location != null)
            {
                Latitude = location.Latitude;
                Longitude = location.Longitude;

                MoveMapAction?.Invoke(location);
            }

            if (lat.HasValue && lng.HasValue)
            {
                Latitude = lat.Value;
                Longitude = lng.Value;

                MoveMapAction?.Invoke(new Location(Latitude, Longitude));
                return;
            }
        }

        public Action<Location>? MoveMapAction { get; set; }

        public void UpdateLocation(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        private void OnClose()
        {
            _tcs.TrySetResult(new GoogleMapResponseModel
            {
                Latitude = Latitude,
                Longitude = Longitude
            });
        }

        public Task<GoogleMapResponseModel> WaitForResultAsync()
        {
            return _tcs.Task;
        }
    }
}
