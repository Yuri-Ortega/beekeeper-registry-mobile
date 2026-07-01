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
using System.Net.Mail;

namespace BeeKeeperRegister.ViewModels
{
    public partial class AddFarmProfileViewModel : ObservableObject
    {
        private readonly IBeeKeeperFarmProfileService _farmProfileService;
        private readonly IBeeLocationProductionTypeSourceService _productionTypeService;
        private readonly IBeeProductioonService _beeProductioonService;
        private readonly IBeeLocationForageService _forageService;
        private readonly IBeeProfileBioSecurityService _bioSecurityService;
        private readonly IOneBAILookupService _lookupService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;
        private readonly TempDataBeeSpecies _tempDataBeeSpecies;
        private readonly TempDataBeeProductioon _tempDataBeeProductioon;

        private string LocationId =>
            $"{SelectedBarangay?.Bcode}{LotNo}";

        //Collections
        [ObservableProperty]
        private ObservableCollection<RegionModel> region = new();

        [ObservableProperty]
        private ObservableCollection<ProvinceModel> province = new();

        [ObservableProperty]
        private ObservableCollection<MunicipalityModel> municipality = new();

        [ObservableProperty]
        private ObservableCollection<BarangayModel> barangay = new();

        [ObservableProperty]
        private ObservableCollection<BeeProductionSystemModel> beeProductionSystem = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonPestModel> beeCommonPest = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonDiseasesModel> beeCommonDiseases = new();

        [ObservableProperty]
        private ObservableCollection<BeeProductionSystemModel> selectedBeeProductionSystem = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonPestModel> selectedBeeCommonPest = new();

        [ObservableProperty]
        private ObservableCollection<BeeCommonDiseasesModel> selectedBeeCommonDiseases = new();

        [ObservableProperty]
        private ObservableCollection<BeeForagesModel> beeLocationForage = new();

        [ObservableProperty]
        private ObservableCollection<BeeForagesModel> selectedBeeLocationForage = new();

        [ObservableProperty]
        private ObservableCollection<BioSecurityUIModel> bioSecurityList = new();

        [ObservableProperty]
        private ObservableCollection<BeeSpeciesUIModel> speciesList = new();

        [ObservableProperty]
        private ObservableCollection<BeeProductioonUIModel> beeProductioonList = new();

        // Selected Items
        [ObservableProperty]
        private RegionModel? selectedRegion;

        [ObservableProperty]
        private ProvinceModel? selectedProvince;

        [ObservableProperty]
        private MunicipalityModel? selectedMunicipality;

        [ObservableProperty]
        private BarangayModel? selectedBarangay;

        // Farm Profile Fields
        [ObservableProperty]
        private int lotNo;

        [ObservableProperty]
        private int currentStep;

        [ObservableProperty]
        private double latitude;

        [ObservableProperty]
        private double longitude;

        [ObservableProperty]
        private string selectedHDMng = "No";

        // UI State
        [ObservableProperty]
        private bool isEnabledProvince;

        [ObservableProperty]
        private bool isEnabledMunicipality;

        [ObservableProperty]
        private bool isEnabledBarangay;

        [ObservableProperty]
        private bool isEnabledSelectLocation;

        [ObservableProperty]
        private bool isEnabledAddBeeSpecies;

        [ObservableProperty]
        private bool isEnabledAddBeeProduction;

        [ObservableProperty]
        private bool speciesListIsEmpty = true;

        [ObservableProperty]
        private bool beeSpeciesHasData;

        [ObservableProperty]
        private bool isRefreshingSpeciesList = false;

        [ObservableProperty]
        private bool beeProductioonListIsEmpty = true;

        [ObservableProperty]
        private bool beeProductioonHasData;

        [ObservableProperty]
        private bool isRefreshingBeeProductioonList = false;

        // Validation Flags
        [ObservableProperty]
        private bool errLotNoBool;

        [ObservableProperty]
        private bool errRegionBool;

        [ObservableProperty]
        private bool errProvinceBool;

        [ObservableProperty]
        private bool errMunicipalityBool;

        [ObservableProperty]
        private bool errBarangayBool;

        [ObservableProperty]
        private bool errBeeProductionSystemBool;

        [ObservableProperty]
        private bool errBeeLocationForageBool;

        [ObservableProperty] private Color errLocationColor;

        public AddFarmProfileViewModel(
            IBeeKeeperFarmProfileService farmProfileService,
            IBeeLocationProductionTypeSourceService productionTypeService,
            IBeeProductioonService beeProductioonService,
            IBeeLocationForageService forageService,
            IBeeProfileBioSecurityService bioSecurityService,
            IOneBAILookupService lookupService,
            ILoadingPopupService loading,
            IDialogPopupService popupService,
            IAHWDLookupService ahwdLookupService,
        TempDataBeeSpecies tempDataBeeSpecies,
            TempDataBeeProductioon tempDataBeeProductioon)
        {
            _farmProfileService = farmProfileService;
            _productionTypeService = productionTypeService;
            _beeProductioonService = beeProductioonService;
            _forageService = forageService;
            _bioSecurityService = bioSecurityService;
            _lookupService = lookupService;
            _loading = loading;
            _popupService = popupService;
            _ahwdLookupService = ahwdLookupService;
            _tempDataBeeSpecies = tempDataBeeSpecies;
            _tempDataBeeProductioon = tempDataBeeProductioon;
        }

        // Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            await Task.WhenAll(
                LoadRegionsAsync(),
                LoadBeeProductionSystemsAsync(),
                LoadBeeCommonPestsAsync(),
                LoadBeeCommonDiseasesAsync(),
                LoadBeeForagesAsync(),
                LoadBioSecurityAsync()
            );

            LoadTempBeeSpecies();
            LoadTempBeeProductioon();
        }

        private async Task LoadRegionsAsync()
        {
            var regions = await _lookupService.GetRegionsAsync();
            if (regions is null) return;
            Region.Clear();
            foreach (var item in regions) Region.Add(item!);
            ErrRegionBool = false;
        }

        private async Task LoadBeeProductionSystemsAsync()
        {
            var systems = await _ahwdLookupService.GetAllBeeProductionSystemsAsync();
            if (systems is null) return;
            BeeProductionSystem.Clear();
            foreach (var item in systems) BeeProductionSystem.Add(item!);
            ErrBeeProductionSystemBool = false;
        }

        private async Task LoadBeeCommonPestsAsync()
        {
            var pests = await _ahwdLookupService.GetAllBeeCommonPestsAsync();
            if (pests is null) return;
            BeeCommonPest.Clear();
            foreach (var item in pests) BeeCommonPest.Add(item!);
        }

        private async Task LoadBeeCommonDiseasesAsync()
        {
            var diseases = await _ahwdLookupService.GetAllBeeCommonDiseasesAsync();
            if (diseases is null) return;
            BeeCommonDiseases.Clear();
            foreach (var item in diseases) BeeCommonDiseases.Add(item!);
        }

        private async Task LoadBeeForagesAsync()
        {
            var forages = await _ahwdLookupService.GetAllBeeForagesAsync();
            if (forages is null) return;
            BeeLocationForage.Clear();
            foreach (var item in forages) BeeLocationForage.Add(item!);
        }

        private async Task LoadBioSecurityAsync()
        {
            var bioSecurity = await _ahwdLookupService.GetAllBeeBiosecuritiesAsync();
            if (bioSecurity is null) return;
            BioSecurityList.Clear();
            foreach (var item in bioSecurity)
            {
                BioSecurityList.Add(new BioSecurityUIModel
                {
                    BeeBioCode = item.BeeBioCode,
                    BeeBioDescription = item.BeeBioDescription
                });
            }
        }

        [RelayCommand]
        public async Task PullToRefreshSpeciesListAsync()
        {
            await Task.Delay(1000);
            LoadTempBeeSpecies();
            IsRefreshingSpeciesList = false;
        }

        private void LoadTempBeeSpecies()
        {
            var tempSpecies = _tempDataBeeSpecies.GetAllBeeSpecies();
            SpeciesListIsEmpty = !tempSpecies.Any();
            BeeSpeciesHasData = tempSpecies.Any();
            SpeciesList.Clear();
            foreach (var item in tempSpecies)
            {
                SpeciesList.Add(new BeeSpeciesUIModel
                {
                    Id = item.Id,
                    BeeTypeDescription = FilterHandler.RemoveParenthesis(item.BeeTypeDescription),
                    Origin = $"{item.ProvinceName}{item.CountryName}",
                    NOC = item.NumberColonies,
                    Source = item.BscoloniesDescription,
                    Imported = item.IfImported == true ? "Yes" : "No"
                });
            }
        }

        [RelayCommand]
        public async Task PullToRefreshBeeProductioonListAsync()
        {
            await Task.Delay(1000);
            LoadTempBeeProductioon();
            IsRefreshingBeeProductioonList = false;
        }

        private void LoadTempBeeProductioon()
        {
            var tempProductioon = _tempDataBeeProductioon.GetAllBeeProductioon();
            BeeProductioonListIsEmpty = !tempProductioon.Any();
            BeeProductioonHasData = tempProductioon.Any();
            BeeProductioonList.Clear();
            foreach (var item in tempProductioon)
            {
                BeeProductioonList.Add(new BeeProductioonUIModel
                {
                    Id = item.Id,
                    BeeProduction = item.BeeProductionDescription,
                    EstProdYield = item.EstProdYield
                });
            }
        }

        // Add Farm Command
        [RelayCommand]
        private async Task AddFarmAsync()
        {
            try
            {
                using (await _loading.Show())
                {
                    var isCreated = await _farmProfileService
                        .AddFarmProfileAsync(BuildFarmProfileModel());

                    if (!isCreated) return;

                    await SaveBeeSpeciesAsync();
                    await SaveBeeProductioonAsync();
                    await SaveForagesAsync();
                    await SaveBioSecurityAsync();

                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private AddBeeKeeperFarmProfileLocationModel BuildFarmProfileModel() => new()
        {
            Rcode = SelectedRegion!.Rcode,
            Regions = SelectedRegion.Region!,
            ProvCode = SelectedProvince!.ProvCode,
            Provinces = SelectedProvince.ProvinceName!,
            MunCode = SelectedMunicipality!.MunCode,
            Municipalities = SelectedMunicipality.MunCity!,
            LotNo = LotNo,
            Bcode = SelectedBarangay!.Bcode,
            Barangay = SelectedBarangay.BarangayName!,
            Latitude = Latitude.ToString(),
            Longitude = Longitude.ToString(),
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
        };

        private async Task SaveBeeSpeciesAsync()
        {
            var tempSpecies = _tempDataBeeSpecies.GetAllBeeSpecies();
            if (!tempSpecies.Any()) return;

            foreach (var item in tempSpecies)
                await _productionTypeService.AddBeeLocationProductionTypeSourceAsync(
                    new AddBeeLocationProductionTypeSourceModel
                    {
                        LocationId = item.LocationId,
                        BeeTypeId = item.BeeTypeId,
                        BeeTypeDescription = item.BeeTypeDescription,
                        NumberColonies = item.NumberColonies,
                        Bscolonies = item.Bscolonies,
                        BscoloniesDescription = item.BscoloniesDescription,
                        ProvCode = item.ProvCode ?? string.Empty,
                        ProvinceName = item.ProvinceName ?? string.Empty,
                        IfImported = item.IfImported,
                        Gmicntry = item.Gmicntry ?? string.Empty,
                        CountryName = item.CountryName ?? string.Empty
                    });
        }

        private async Task SaveBeeProductioonAsync()
        {
            var tempProductioon = _tempDataBeeProductioon.GetAllBeeProductioon();
            if (!tempProductioon.Any()) return;

            foreach (var item in tempProductioon)
                await _beeProductioonService.AddBeeProductioonAsync(
                    new BeeProductioonModel
                    {
                        LocationId = item.LocationId,
                        BeeProdId = item.BeeProdId,
                        BeeProductionDescription = item.BeeProductionDescription,
                        EstProdYield = item.EstProdYield
                    });
        }

        private async Task SaveForagesAsync()
        {
            foreach (var forage in SelectedBeeLocationForage)
                await _forageService.AddBeeLocationForagesAsync(
                    new AddBeeLocationForageModel
                    {
                        LocationId = LocationId,
                        ForageCode = forage.ForageCode,
                        ForagesDescription = forage.ForagesDescription
                    });
        }

        private async Task SaveBioSecurityAsync()
        {
            foreach (var item in BioSecurityList)
                await _bioSecurityService.AddBeeProfileBiosecurityAsync(
                    new AddBeeProfileBioSecurityModel
                    {
                        LocationId = LocationId,
                        BeeBioCode = item.BeeBioCode,
                        BeeBioDescription = item.BeeBioDescription,
                        Result = item.SelectedBioSecurity
                    });
        }

        // Validation
        public bool ValidateStep()
        {
            return CurrentStep switch
            {
                0 => ValidateStepZero(),
                4 => ValidateStepFour(),
                _ => true
            };
        }

        private bool ValidateStepZero()
        {
            var LongLatRequire = (Longitude == 0 || Latitude == 0);
            ErrLocationColor = LongLatRequire ? Color.FromArgb("#B5402E") : Color.FromArgb("#C9962F");

            ErrRegionBool = SelectedRegion == default;
            ErrProvinceBool = SelectedProvince == default;
            ErrMunicipalityBool = SelectedMunicipality == default;
            ErrBarangayBool = SelectedBarangay == default;
            ErrLotNoBool = LotNo == 0;
            return !(ErrRegionBool || ErrProvinceBool || ErrMunicipalityBool
                     || ErrBarangayBool || ErrLotNoBool || LongLatRequire);
        }

        private bool ValidateStepFour()
        {
            ErrBeeProductionSystemBool = !SelectedBeeProductionSystem.Any();
            ErrBeeLocationForageBool = !SelectedBeeLocationForage.Any();
            return !(ErrBeeLocationForageBool || ErrBeeProductionSystemBool);
        }

        // Selection Events
        [RelayCommand]
        public async Task SelectionRegionAsync()
        {
            if (SelectedRegion == default) return;
            IsEnabledProvince = true;
            ErrRegionBool = false;
            Province.Clear();

            var provinces = await _lookupService
                .GetProvincesByRcodeAsync(SelectedRegion.Rcode);
            if (provinces is null) return;

            foreach (var item in provinces) Province.Add(item);
            SelectedProvince = null;
            SelectedMunicipality = null;
            SelectedBarangay = null;
        }

        [RelayCommand]
        public async Task SelectionProvinceAsync()
        {
            if (SelectedProvince == default) return;
            IsEnabledMunicipality = true;
            ErrProvinceBool = false;
            Municipality.Clear();

            var municipalities = await _lookupService
                .GetMunicipalitiesByProvCodeAsync(SelectedProvince.ProvCode);
            if (municipalities is null) return;

            foreach (var item in municipalities) Municipality.Add(item);
            SelectedMunicipality = null;
            SelectedBarangay = null;
        }

        [RelayCommand]
        public async Task SelectionMunicipalityAsync()
        {
            if (SelectedMunicipality == default) return;
            IsEnabledBarangay = true;
            ErrMunicipalityBool = false;
            Barangay.Clear();

            var barangays = await _lookupService
                .GetBarangaysByMunCodeAsync(SelectedMunicipality.MunCode);
            if (barangays is null) return;

            foreach (var item in barangays) Barangay.Add(item);
            SelectedBarangay = null;
        }

        [RelayCommand]
        public async Task SelectionBarangayAsync()
        {
            if (SelectedBarangay is null)
            {
                IsEnabledSelectLocation = false;
                return;
            }

            if (LotNo != 0)
            {
                IsEnabledAddBeeSpecies = true;
                IsEnabledAddBeeProduction = true;
            }

            ErrBarangayBool = false;
            Latitude = 0.0;
            Longitude = 0.0;
            IsEnabledSelectLocation = true;
        }

        [RelayCommand]
        public async Task SelectionBeeProductionSystemAsync()
        {
            if (!SelectedBeeProductionSystem.Any()) return;
            ErrBeeProductionSystemBool = false;
        }

        [RelayCommand]
        public async Task SelectionBeeCommonPestAsync()
        {
            if (!SelectedBeeCommonPest.Any()) return;
        }

        [RelayCommand]
        public async Task SelectionBeeCommonDiseasesAsync()
        {
            if (!SelectedBeeCommonDiseases.Any()) return;
        }

        [RelayCommand]
        public async Task SelectionBeeLocationForageAsync()
        {
            if (!SelectedBeeLocationForage.Any()) return;
            ErrBeeLocationForageBool = false;
        }

        // Google Map Command
        [RelayCommand]
        private async Task MapLocationBtnAsync()
        {
            if (SelectedBarangay == null || SelectedMunicipality == null ||
                SelectedProvince == null || SelectedRegion == null) return;

            var result = await _popupService.ShowGoogleMapPopup(
                SelectedRegion.Region!,
                SelectedProvince.ProvinceName!,
                SelectedMunicipality.MunCity!,
                SelectedBarangay.BarangayName!);

            if (result != null)
            {
                Latitude = result.Latitude;
                Longitude = result.Longitude;
                ErrLocationColor = Color.FromArgb("#C9962F");
            }
        }

        //Bee Species Command
        [RelayCommand]
        private async Task AddBeeSpeciesAsync() =>
            await Shell.Current.GoToAsync(nameof(AddTempBeeSpeciesPage),
                new Dictionary<string, object> { { "LocationId", LocationId } });

        [RelayCommand]
        public async Task UpdateBeeSpeciesAsync(BeeSpeciesUIModel item)
        {
            if (item is null) return;
            await Shell.Current.GoToAsync(nameof(UpdateTempBeeSpeciesPage),
                new Dictionary<string, object> { { "Id", item.Id } });
        }

        [RelayCommand]
        public async Task DeleteBeeSpeciesAsync(BeeSpeciesUIModel item)
        {
            bool isYes = await _popupService
                .ShowConfirmDialog("Delete","Are you sure you want to delete?", "Yes", "No");
            if (!isYes) return;

            using (await _loading.Show())
            {
                var isRemoved = _tempDataBeeSpecies.DeleteBeeSpecies(item.Id);
                if (isRemoved)
                    _popupService.ShowSuccessDialog(
                        $"{item.BeeTypeDescription} deleted successfully");

                if (!_tempDataBeeSpecies.GetAllBeeSpecies().Any())
                {
                    SpeciesListIsEmpty = true;
                    BeeSpeciesHasData = false;
                }

                _ = LoaderAsync();
            }
        }

        //Bee Productioon Command
        [RelayCommand]
        private async Task AddBeeProductioonAsync() =>
            await Shell.Current.GoToAsync(nameof(AddTempBeeProductioonPage),
                new Dictionary<string, object> { { "LocationId", LocationId } });

        [RelayCommand]
        public async Task UpdateBeeProductioonAsync(BeeProductioonUIModel item)
        {
            if (item is null) return;
            await Shell.Current.GoToAsync(nameof(UpdateTempBeeProductioonPage),
                new Dictionary<string, object> { { "Id", item.Id } });
        }

        [RelayCommand]
        public async Task DeleteBeeProductioonAsync(BeeProductioonUIModel item)
        {
            bool isYes = await _popupService
                .ShowConfirmDialog("Delete", "Are you sure you want to delete?", "Yes", "No");
            if (!isYes) return;

            using (await _loading.Show())
            {
                var isRemoved = _tempDataBeeProductioon.DeleteBeeProductioon(item.Id);
                if (isRemoved)
                    _popupService.ShowSuccessDialog(
                        $"{item.BeeProduction} deleted successfully");

                if (!_tempDataBeeProductioon.GetAllBeeProductioon().Any())
                {
                    BeeProductioonListIsEmpty = true;
                    BeeProductioonHasData = false;
                }

                _ = LoaderAsync();
            }
        }

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

        // Property Changed Handlers
        partial void OnLotNoChanged(int newValue)
        {
            ErrLotNoBool = newValue == 0;
            _tempDataBeeSpecies.ClearAll();

            if (newValue != 0 && SelectedBarangay != null)
            {
                IsEnabledAddBeeSpecies = true;
                IsEnabledAddBeeProduction = true;
            }
        }

        // Reset Form
        public void ResetForm()
        {
            SelectedRegion = new();
            SelectedProvince = new();
            SelectedMunicipality = new();
            SelectedBarangay = new();
            SelectedBeeProductionSystem = new();
            SelectedBeeCommonPest = new();
            SelectedBeeCommonDiseases = new();
            SelectedBeeLocationForage = new();
            LotNo = 0;
            Latitude = 0.0;
            Longitude = 0.0;
            IsEnabledSelectLocation = false;
            IsEnabledProvince = false;
            IsEnabledMunicipality = false;
            IsEnabledBarangay = false;
            SpeciesListIsEmpty = true;
            BeeSpeciesHasData = false;
            SpeciesList.Clear();
            _tempDataBeeSpecies.ClearAll();
            BeeProductioonListIsEmpty = true;
            BeeProductioonHasData = false;
            BeeProductioonList.Clear();
            _tempDataBeeProductioon.ClearAll();
            ErrLotNoBool = ErrRegionBool = ErrProvinceBool =
            ErrBeeLocationForageBool = ErrMunicipalityBool =
            ErrBarangayBool = ErrBeeProductionSystemBool = false;
            CurrentStep = 0;
            _popupService.ShowSuccessDialog("Farm profile added successfully");
        }
    }
}