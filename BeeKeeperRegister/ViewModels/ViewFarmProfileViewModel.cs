using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Models.UI;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using BeeKeeperRegister.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BeeKeeperRegister.ViewModels
{
    public partial class ViewFarmProfileViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeKeeperFarmProfileService _farmProfileService;
        private readonly IBeeLocationForageService _forageService;
        private readonly IBeeLocationProductionTypeSourceService _productionTypeService;
        private readonly IBeeProductioonService _beeProductioonService;
        private readonly IBeeProfileBioSecurityService _bioSecurityService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;


        // Query Attributes
        [ObservableProperty]
        private string locationId = string.Empty;

        [ObservableProperty]
        private string tempLocationId = string.Empty;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("LocationId", out var value))
                LocationId = value!.ToString()!;
        }


        // Farm Profile Fields
        [ObservableProperty]
        private int lotNo;

        [ObservableProperty]
        private string location = string.Empty;

        [ObservableProperty]
        private string productionSystem = string.Empty;

        [ObservableProperty]
        private string disease = string.Empty;

        [ObservableProperty]
        private string pest = string.Empty;

        [ObservableProperty]
        private string hDMng = string.Empty;

        [ObservableProperty]
        private string forages = string.Empty;


        // Collections
        [ObservableProperty]
        private ObservableCollection<ViewFarmBeeSpeciesUIModel> speciesList = new();

        [ObservableProperty]
        private ObservableCollection<ViewFarmBeeProductioonUIModel> beeProductioonList = new();

        [ObservableProperty]
        private ObservableCollection<BioSecurityUIModel> bioSecurityList = new();


        // UI State
        [ObservableProperty]
        private bool speciesListIsEmpty = true;

        [ObservableProperty]
        private bool beeSpeciesHasData;

        [ObservableProperty]
        private bool beeProductioonListIsEmpty = true;

        [ObservableProperty]
        private bool beeProductioonHasData;

        [ObservableProperty]
        private bool isRefreshing = false;

        [ObservableProperty]
        private bool inviYesBio;

        [ObservableProperty]
        private bool readonlyYesBio;

        [ObservableProperty]
        private bool inviNoBio;

        [ObservableProperty]
        private bool readonlyNoBio;

        [ObservableProperty]
        private string editBioButton = "Edit";

        public ViewFarmProfileViewModel(
            IBeeKeeperFarmProfileService farmProfileService,
            IBeeLocationForageService forageService,
            IBeeLocationProductionTypeSourceService productionTypeService,
            IBeeProductioonService beeProductioonService,
            IBeeProfileBioSecurityService bioSecurityService,
            ILoadingPopupService loading,
            IDialogPopupService popupService)
        {
            _farmProfileService = farmProfileService;
            _forageService = forageService;
            _productionTypeService = productionTypeService;
            _beeProductioonService = beeProductioonService;
            _bioSecurityService = bioSecurityService;
            _loading = loading;
            _popupService = popupService;
        }


        // Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            await LoadFarmProfile();
            await LoadBeeSpecies();
            await LoadBeeProductioon();
            await LoadBeeBioSecurity();
        }

        [RelayCommand]
        public async Task RefreshFarmProfileAsync()
        {
            await Task.Delay(1000);
            await LoadFarmProfile();
            await LoadBeeSpecies();
            await LoadBeeProductioon();
            await LoadBeeBioSecurity();
            IsRefreshing = false;
        }

        //Farm Profile
        private async Task LoadFarmProfile()
        {
            var farmTask = _farmProfileService.GetFarmProfileByLocationIdAsync(LocationId);
            var forageTask = _forageService
                .GetAllBeeLocationForagesByLocationIdAsync(LocationId);

            await Task.WhenAll(
                farmTask, forageTask);

            var farm = await farmTask;
            var forageLocation = await forageTask;

            if (farm is null || forageLocation is null) return;


            //Farm info
            TempLocationId = $"{farm.Bcode}{farm.LotNo}";
            LotNo = farm.LotNo;
            Location = $"{farm.Barangay}, {farm.Municipalities}, {farm.Provinces}";
            ProductionSystem = farm.BeeSystemProduction;
            Disease = string.IsNullOrEmpty(farm.CommonDiseaseBeeDescription)
                ? "None" : farm.CommonDiseaseBeeDescription;
            Pest = string.IsNullOrEmpty(farm.CommonPestsDescription)
                ? "None" : farm.CommonPestsDescription;
            HDMng = farm.Hdmng ? "Yes" : "No";
            Forages = string.Join(", ",
                forageLocation.Select(u => u.ForagesDescription));

            await InitializeLocationAsync(
                farm.Regions, farm.Provinces,
                farm.Municipalities, farm.Barangay,
                Convert.ToDouble(farm.Latitude),
                Convert.ToDouble(farm.Longitude));
        }

        //Bee Biosecurity
        private async Task LoadBeeBioSecurity(bool? tempVisibleYesBio = false, bool? tempVisibleNoBio = false)
        {
            var bioSecurity = await _bioSecurityService.GetAllBeeProfileBiosecurityByLocationIdAsync(LocationId);
            if (!bioSecurity!.Any()) return;
            BioSecurityList.Clear();
            foreach (var item in bioSecurity!)
            {
                BioSecurityList.Add(new BioSecurityUIModel
                {
                    SelectedBioSecurity = item.Result,
                    BeeBioCode = item.BeeBioCode,
                    BeeBioDescription = item.BeeBioDescription!,
                    IsviYesBio = tempVisibleYesBio == true ? tempVisibleYesBio : item.Result,
                    IsviNoBio = tempVisibleNoBio == true ? tempVisibleNoBio : !item.Result,
                    ColumnNumNoBio = tempVisibleNoBio == true ? 1 : (!item.Result == true ? 0 : 1)
                });
            }
        }

        //Species
        private async Task LoadBeeSpecies()
        {
            var species = await _productionTypeService.GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(LocationId);
            SpeciesListIsEmpty = !species.Any();
            if (!species.Any()) return;
            BeeSpeciesHasData = species.Any();

            SpeciesList.Clear();
            foreach (var item in species)
            {
                SpeciesList.Add(new ViewFarmBeeSpeciesUIModel
                {
                    BeeProdCtr = item!.BeeProdCtr,
                    BeeTypeDescription = item.BeeTypeDescription,
                    ShortBeeTypeDescription = FilterHandler.RemoveParenthesis(item.BeeTypeDescription),
                    Origin = $"{item.ProvinceName}{item.CountryName}",
                    NOC = item.NumberColonies,
                    Source = item.BscoloniesDescription!,
                    Imported = item.IfImported == true ? "Yes" : "No"
                });
            }
        }

        // Species Commands
        [RelayCommand]
        private async Task AddBeeSpeciesAsync() =>
            await Shell.Current.GoToAsync(nameof(AddBeeSpeciesPage),
                new Dictionary<string, object>
                {
                    { "LocationId", TempLocationId }
                });

        [RelayCommand]
        public async Task UpdateBeeSpeciesAsync(ViewFarmBeeSpeciesUIModel item)
        {
            if (item is null) return;
            await Shell.Current.GoToAsync(nameof(UpdateBeeSpeciesPage),
                new Dictionary<string, object>
                {
                    { "BeeProdCtr", item.BeeProdCtr },
                    { "LocationId", TempLocationId }
                });
        }

        [RelayCommand]
        public async Task DeleteBeeSpeciesAsync(ViewFarmBeeSpeciesUIModel item)
        {
            bool isYes = await _popupService
                .ShowConfirmDialog("Delete", "Are you sure you want to delete?", "Yes", "No");
            if (!isYes) return;

            using (await _loading.Show())
            {
                var isRemoved = await _productionTypeService.DeleteBeeLocationProductionTypeSourceByBeeProdCtrAsync(item.BeeProdCtr);
                if (isRemoved)
                    _popupService.ShowSuccessDialog(
                        $"{item.BeeTypeDescription} deleted successfully");

                var remaining = await _productionTypeService
                    .GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(TempLocationId);
                SpeciesListIsEmpty = !remaining?.Any() ?? true;
                BeeSpeciesHasData = remaining?.Any() ?? false;

                _ = LoaderAsync();
            }
        }

        //Bee Productioon
        private async Task LoadBeeProductioon()
        {
            var beeProductioon = await _beeProductioonService.GetAllBeeProductioonByLocationIdAsync(LocationId);
            BeeProductioonListIsEmpty = !beeProductioon.Any();
            if (!beeProductioon.Any()) return;
            BeeProductioonHasData = beeProductioon.Any();
            BeeProductioonList.Clear();
            foreach (var item in beeProductioon)
            {
                BeeProductioonList.Add(new ViewFarmBeeProductioonUIModel
                {
                    BeeProdID = item!.BeeProdId,
                    BeeProduction = item.BeeProductionDescription,
                    EstProdYield = item.EstProdYield
                });
            }
        }

        // Productioon Commands
        [RelayCommand]
        private async Task AddBeeProductioonAsync() =>
            await Shell.Current.GoToAsync(nameof(AddBeeProductioonPage),
                new Dictionary<string, object>
                {
                    { "LocationId", TempLocationId }
                });

        [RelayCommand]
        public async Task UpdateBeeProductioonAsync(ViewFarmBeeProductioonUIModel item)
        {
            if (item is null) return;
            await Shell.Current.GoToAsync(nameof(UpdateBeeProductioonPage),
                new Dictionary<string, object>
                {
                    { "BeeProdID", item.BeeProdID },
                    { "LocationId", TempLocationId }
                });
        }

        [RelayCommand]
        public async Task DeleteBeeProductioonAsync(ViewFarmBeeProductioonUIModel item)
        {
            bool isYes = await _popupService
                .ShowConfirmDialog("Delete", "Are you sure you want to delete?", "Yes", "No");
            if (!isYes) return;

            using (await _loading.Show())
            {
                var isRemoved = await _beeProductioonService.DeleteBeeProductioonByBeeProdIdAsync(item.BeeProdID);
                if (isRemoved)
                    _popupService.ShowSuccessDialog(
                        $"{item.BeeProduction} deleted successfully");

                var remaining = await _beeProductioonService
                    .GetAllBeeProductioonByLocationIdAsync(TempLocationId);
                BeeProductioonListIsEmpty = !remaining?.Any() ?? true;
                BeeProductioonHasData = remaining?.Any() ?? false;

                _ = LoaderAsync();
            }
        }


        //Google Map Action
        public Action<Location>? MoveMapAction { get; set; }

        private async Task InitializeLocationAsync(
            string region, string province,
            string municipality, string barangay,
            double? latitude = null, double? longitude = null)
        {
            if (string.IsNullOrEmpty(barangay)) return;

            if (latitude.HasValue && longitude.HasValue)
            {
                MoveMapAction?.Invoke(
                    new Location(latitude.Value, longitude.Value));
                return;
            }

            var address =
                $"{barangay}, {municipality}, {province}, {region}, Philippines";
            var locations = await Geocoding.GetLocationsAsync(address);
            var location = locations?.FirstOrDefault();

            if (location != null)
                MoveMapAction?.Invoke(location);
        }

        // Edit Farm Profile Command
        [RelayCommand]
        private async Task EditFarmProfileAsync() =>
            await Shell.Current.GoToAsync(nameof(UpdateFarmProfilePage),
                new Dictionary<string, object>
                {
                    { "LocationId", TempLocationId }
                });

        // BioSecurity Selection Commands
        [RelayCommand]
        public void SelectYesBio(BioSecurityUIModel item)
        {
            if (item is null) return;
            item.SelectedBioSecurity = true;
        }

        [RelayCommand]
        public void SelectNoBio(BioSecurityUIModel item)
        {
            if (item is null) return;
            item.SelectedBioSecurity = false;
        }

        [RelayCommand]
        public async Task EditBio()
        {
            if(EditBioButton == "Edit")
            {
                EditBioButton = "Save";
                ReadonlyNoBio = false;
                ReadonlyYesBio = false;
                await LoadBeeBioSecurity(true, true);

            } else
            {
                EditBioButton = "Edit";
                ReadonlyNoBio = true;
                ReadonlyYesBio = true;

                using (await _loading.Show())
                {
                    _popupService.ShowSuccessDialog("Biosecurity Measures updated successfully");
                    foreach (var item in BioSecurityList)
                        await _bioSecurityService.UpdateBeeProfileBiosecurityAsync(
                            new UpdateBeeProfileBioSecurityModel
                            {
                                LocationId = LocationId,
                                BeeBioCode = item.BeeBioCode,
                                Result = item.SelectedBioSecurity
                            });

                    await LoadBeeBioSecurity(false, false);
                }

            }

        }
    }
}