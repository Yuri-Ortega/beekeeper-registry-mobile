using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using BeeKeeperRegister.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class UpdateFarmProfileViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeKeeperFarmProfileService _farmProfileService;
        private readonly IBeeLocationForageService _forageService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;

        public Action<Location>? MoveMapAction { get; set; }


        // Query
        [ObservableProperty]
        private string locationId = string.Empty;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("LocationId", out var value))
                LocationId = value?.ToString() ?? string.Empty;
        }


        // Farm profile Fields
        [ObservableProperty] private string selectedHDMng = "No";
        [ObservableProperty] private int lotNo;
        [ObservableProperty] private string location = string.Empty;
        [ObservableProperty] private double selectedLatitude;
        [ObservableProperty] private double selectedLongitude;


        // Error Flags
        [ObservableProperty] private bool errBeeProductionSystemBool;
        [ObservableProperty] private bool errBeeLocationForageBool;


        // Collections
        [ObservableProperty]
        private ObservableCollection<BeeProductionSystemModel> beeProductionSystem = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonPestModel> beeCommonPest = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonDiseasesModel> beeCommonDiseases = new();

        [ObservableProperty]
        private ObservableCollection<BeeForagesModel> beeLocationForage = new();

        [ObservableProperty]
        private ObservableCollection<BeeProductionSystemModel> selectedBeeProductionSystem = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonPestModel> selectedBeeCommonPest = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonDiseasesModel> selectedBeeCommonDiseases = new();

        [ObservableProperty]
        private ObservableCollection<BeeForagesModel> selectedBeeLocationForage = new();

        private Task<List<BeeLocationForageModel>?> forageLocationTask;

        public UpdateFarmProfileViewModel(
            IBeeKeeperFarmProfileService farmProfileService,
            IBeeLocationForageService forageService,
            ILoadingPopupService loading,
            IDialogPopupService popupService,
            IAHWDLookupService ahwdLookupService)
        {
            _farmProfileService = farmProfileService;
            _forageService = forageService;
            _loading = loading;
            _popupService = popupService;
            _ahwdLookupService = ahwdLookupService;
        }


        // Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var farmTask = _farmProfileService.GetFarmProfileByLocationIdAsync(LocationId);
            forageLocationTask = _forageService.GetAllBeeLocationForagesByLocationIdAsync(LocationId);
            var productionSystemTask = _ahwdLookupService.GetAllBeeProductionSystemsAsync();
            var commonPestTask = _ahwdLookupService.GetAllBeeCommonPestsAsync();
            var commonDiseasesTask = _ahwdLookupService.GetAllBeeCommonDiseasesAsync();
            var foragesTask = _ahwdLookupService.GetAllBeeForagesAsync();

            await Task.WhenAll(
                farmTask, forageLocationTask, productionSystemTask,
                commonPestTask, commonDiseasesTask, foragesTask);

            var farm = await farmTask;
            var forageLocation = await forageLocationTask;
            var productionSystem = await productionSystemTask;
            var commonPest = await commonPestTask;
            var commonDiseases = await commonDiseasesTask;
            var forages = await foragesTask;

            if (farm is null || forageLocation is null ||
                productionSystem is null || commonPest is null ||
                commonDiseases is null || forages is null) return;

            BeeProductionSystem.Clear();
            ErrBeeProductionSystemBool = false;
            foreach (var item in productionSystem) BeeProductionSystem.Add(item!);

            BeeCommonPest.Clear();
            foreach (var item in commonPest) BeeCommonPest.Add(item!);

            BeeCommonDiseases.Clear();
            foreach (var item in commonDiseases) BeeCommonDiseases.Add(item!);

            BeeLocationForage.Clear();
            foreach (var item in forages) BeeLocationForage.Add(item!);

            LotNo = farm.LotNo;
            Location = $"{farm.Barangay}, {farm.Municipalities}, {farm.Provinces}";
            SelectedHDMng = farm.Hdmng ? "Yes" : "No";
            SelectedLatitude = Convert.ToDouble(farm.Latitude);
            SelectedLongitude = Convert.ToDouble(farm.Longitude);

            await InitializeLocationAsync(
                farm.Regions, farm.Provinces,
                farm.Municipalities, farm.Barangay,
                Convert.ToDouble(farm.Latitude),
                Convert.ToDouble(farm.Longitude));

            SelectedBeeProductionSystem = new ObservableCollection<BeeProductionSystemModel>(
                BeeProductionSystem.Where(x =>
                    FilterHandler.FilterMultipleSelectionTokenEdit(farm.BeeSystemProduction)
                        .Contains(x.BeeSystemProduction?.Trim().ToLower())));

            SelectedBeeCommonDiseases = new ObservableCollection<BeeCommonDiseasesModel>(
                BeeCommonDiseases.Where(x =>
                    FilterHandler.FilterMultipleSelectionTokenEdit(farm.CommonDiseaseBeeDescription)
                        .Contains(x.CommonDiseaseBeeDescription?.Trim().ToLower())));

            SelectedBeeCommonPest = new ObservableCollection<BeeCommonPestModel>(
                BeeCommonPest.Where(x =>
                    FilterHandler.FilterMultipleSelectionTokenEdit(farm.CommonPestsDescription)
                        .Contains(x.CommonPestsDescription?.Trim().ToLower())));

            var filterBeeForage = forageLocation
                .Where(x => !string.IsNullOrEmpty(x.ForagesDescription))
                .SelectMany(x => x.ForagesDescription!
                    .Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Trim())
                .ToHashSet();

            SelectedBeeLocationForage = new ObservableCollection<BeeForagesModel>(
                BeeLocationForage.Where(x =>
                    filterBeeForage.Contains(x.ForagesDescription!)));
        }


        // Selection Events
        [RelayCommand]
        public async Task SelectionBeeLocationForageAsync()
        {
            if (!SelectedBeeLocationForage.Any()) return;
            ErrBeeLocationForageBool = false;
        }

        [RelayCommand]
        public async Task SelectionBeeProductionSystemAsync()
        {
            if (SelectedBeeProductionSystem.Any() == false) return;
            ErrBeeProductionSystemBool = false;
        }

        [RelayCommand]
        public async Task SelectionBeeCommonPestAsync()
        {
            if (SelectedBeeCommonPest.Any() == false) return;
        }

        [RelayCommand]
        public async Task SelectionBeeCommonDiseasesAsync()
        {
            if (SelectedBeeCommonDiseases.Any() == false) return;
        }


        //Google Map
        [RelayCommand]
        private async Task MapLocationBtnAsync()
        {
            try
            {
                var farm = await _farmProfileService
                    .GetFarmProfileByLocationIdAsync(LocationId);
                if (farm?.Barangay == null || farm.Municipalities == null ||
                    farm.Provinces == null || farm.Regions == null) return;

                var result = await _popupService.ShowGoogleMapPopup(
                    farm.Regions!, farm.Provinces!,
                    farm.Municipalities!, farm.Barangay!,
                    Convert.ToDouble(farm.Latitude),
                    Convert.ToDouble(farm.Longitude));

                if (result == null) return;

                SelectedLatitude = result.Latitude;
                SelectedLongitude = result.Longitude;
                MoveMapAction?.Invoke(
                    new Location(result.Latitude, result.Longitude));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task InitializeLocationAsync(
            string region, string province,
            string municipality, string barangay,
            double? latitude = null, double? longitude = null)
        {
            if (string.IsNullOrEmpty(barangay)) return;

            if (latitude.HasValue && longitude.HasValue)
            {
                SelectedLatitude = latitude.Value;
                SelectedLongitude = longitude.Value;
                MoveMapAction?.Invoke(
                    new Location(latitude.Value, longitude.Value));
                return;
            }

            var address =
                $"{barangay}, {municipality}, {province}, {region}, Philippines";
            var locations = await Geocoding.GetLocationsAsync(address);
            var location = locations?.FirstOrDefault();

            if (location == null) return;

            SelectedLatitude = location.Latitude;
            SelectedLongitude = location.Longitude;
            MoveMapAction?.Invoke(location);
        }


        // Update farm profile command
        [RelayCommand]
        private async Task UpdateFarmProfileAsync()
        {
            try
            {
                ErrBeeProductionSystemBool = !SelectedBeeProductionSystem.Any();
                ErrBeeLocationForageBool = !SelectedBeeLocationForage.Any();
                if (ErrBeeLocationForageBool || ErrBeeProductionSystemBool) return;

                using (await _loading.Show())
                {
                    await SaveForagesAsync();

                    var isUpdated = await _farmProfileService.UpdateFarmProfileAsync(
                        new UpdateBeeKeeperFarmProfileLocationModel
                        {
                            LocationId = LocationId,
                            Latitude = SelectedLatitude.ToString(),
                            Longitude = SelectedLongitude.ToString(),
                            Hdmng = SelectedHDMng == "Yes",
                            BeeProSysId = string.Join(", ",
                                SelectedBeeProductionSystem.Select(x => x.BeeProSysId)),
                            BeeSystemProduction = string.Join(", ",
                                SelectedBeeProductionSystem.Select(x => x.BeeSystemProduction)),
                            CommonDiseaseBee = string.Join(", ",
                                SelectedBeeCommonDiseases.Select(x => x.CommonDiseaseBee)),
                            CommonDiseaseBeeDescription = string.Join(", ",
                                SelectedBeeCommonDiseases.Select(x => x.CommonDiseaseBeeDescription)),
                            CommonPests = string.Join(", ",
                                SelectedBeeCommonPest.Select(x => x.CommonPests)),
                            CommonPestsDescription = string.Join(", ",
                                SelectedBeeCommonPest.Select(x => x.CommonPestsDescription))
                        });

                    if (!isUpdated) return;

                    _popupService.ShowSuccessDialog("Farm profile updated successfully");
                    _ = LoaderAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task SaveForagesAsync()
        {
            var forageLocation = await forageLocationTask;
            foreach (var forage in forageLocation)
                await _forageService.DeleteBeeLocationForagesAsync(forage.ForageCode);



            foreach (var forage in SelectedBeeLocationForage)
                await _forageService.AddBeeLocationForagesAsync(
                    new AddBeeLocationForageModel
                    {
                        LocationId = LocationId,
                        ForageCode = forage.ForageCode,
                        ForagesDescription = forage.ForagesDescription
                    });
        }
    }
}