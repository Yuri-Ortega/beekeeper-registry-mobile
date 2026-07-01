using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class AddTempBeeSpeciesViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeLocationProductionTypeSourceService _productionTypeService;
        private readonly IOneBAILookupService _oneBAILookupService;
        private readonly IAHWDLookupService _ahwdLookupService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly TempDataBeeSpecies _tempDataBeeSpecies;

        private string _locationId = string.Empty;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("LocationId", out var value))
                _locationId = value.ToString()!;
        }


        //Bee Species Fields
        [ObservableProperty]
        private int numberOfColonies;

        [ObservableProperty]
        private string selectedImported = "No";

        [ObservableProperty]
        private bool isLocal = true;

        [ObservableProperty]
        private bool isImported;

        [ObservableProperty]
        private bool showBeeType;


        // Collections
        [ObservableProperty]
        private ObservableCollection<BeeTypesModel> beeType = new();

        [ObservableProperty]
        private ObservableCollection<BeeSourceColoniesModel> bsColonies = new();

        [ObservableProperty]
        private ObservableCollection<ProvinceModel> provinceSource = new();

        [ObservableProperty]
        private ObservableCollection<CountryModel> country = new();


        // Selected Items
        [ObservableProperty]
        private BeeTypesModel? selectedBeeType;

        [ObservableProperty]
        private BeeSourceColoniesModel? selectedBsColonies;

        [ObservableProperty]
        private ProvinceModel? selectedProvinceSource;

        [ObservableProperty]
        private CountryModel? selectedCountry;


        // Error Flags
        [ObservableProperty]
        private bool errNumberOfColoniesBool;

        [ObservableProperty]
        private bool errProvinceSourceBool;

        [ObservableProperty]
        private bool errCountryBool;

        [ObservableProperty]
        private bool errBeeTypeBool;

        [ObservableProperty]
        private bool errBsColoniesBool;

        public AddTempBeeSpeciesViewModel(
            IBeeLocationProductionTypeSourceService productionTypeService,
            IOneBAILookupService oneBAIlookupService,
            IAHWDLookupService ahwdLookupService,
            ILoadingPopupService loading,
            TempDataBeeSpecies tempDataBeeSpecies,
            IDialogPopupService popupService)
        {
            _productionTypeService = productionTypeService;
            _oneBAILookupService = oneBAIlookupService;
            _ahwdLookupService = ahwdLookupService;
            _tempDataBeeSpecies = tempDataBeeSpecies;
            _loading = loading;
            _popupService = popupService;
        }

        //loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            await Task.WhenAll(
                LoadBsColoniesAsync(),
                LoadProvinceSourceAsync(),
                LoadCountriesAsync()
            );
        }

        private async Task LoadBsColoniesAsync()
        {
            var data = await _ahwdLookupService.GetAllBeeSourcesColoniesAsync();
            if (data is null) return;
            BsColonies.Clear();
            ErrBsColoniesBool = false;
            foreach (var item in data) BsColonies.Add(item!);
        }

        private async Task LoadProvinceSourceAsync()
        {
            var data = await _oneBAILookupService.GetProvincesAsync();
            if (data is null) return;
            ProvinceSource.Clear();
            ErrProvinceSourceBool = false;
            foreach (var item in data) ProvinceSource.Add(item!);
        }

        private async Task LoadCountriesAsync()
        {
            var data = await _oneBAILookupService.GetCountriesAsync();
            if (data is null) return;
            Country.Clear();
            ErrCountryBool = false;
            foreach (var item in data) Country.Add(item!);
        }


        // Selection Events
        partial void OnSelectedImportedChanged(string value)
        {
            if (value == "Yes")
            {
                SelectedProvinceSource = null;
                SelectedBeeType = null;
                IsImported = true;
                IsLocal = false;
            }
            else
            {
                SelectedCountry = null;
                SelectedBeeType = null;
                IsImported = false;
                IsLocal = true;
            }
        }

        [RelayCommand]
        public async Task SelectionProvinceSourceAsync()
        {
            if (SelectedProvinceSource == null) return;

            await LoadBeeTypesByProvinceAsync();
            ErrProvinceSourceBool = false;
        }

        [RelayCommand]
        public async Task SelectionCountryAsync()
        {
            if (SelectedCountry == null) return;

            await LoadBeeTypesByCountryAsync();
            ErrCountryBool = false;
        }

        [RelayCommand]
        public async Task SelectionBeeTypeAsync()
        {
            if (SelectedBeeType == null) return;
            ErrBeeTypeBool = false;
        }

        [RelayCommand]
        public async Task SelectionBsColoniesAsync()
        {
            if (SelectedBsColonies == null) return;
            ErrBsColoniesBool = false;
        }

        private async Task LoadBeeTypesByProvinceAsync()
        {
            var beeTypes = await _ahwdLookupService.GetAllBeeTypesAsync();
            if (beeTypes is null) return;

            var tempData = _tempDataBeeSpecies.GetAllBeeSpecies();
            BeeType.Clear();
            ErrBeeTypeBool = false;

            foreach (var item in beeTypes)
            {
                if (tempData!.Any(x =>
                    x!.BeeTypeId == item.BeeTypeId &&
                    x.ProvCode == SelectedProvinceSource?.ProvCode))
                    continue;
                BeeType.Add(item!);
            }

            ShowBeeType = BeeType.Any();
        }

        private async Task LoadBeeTypesByCountryAsync()
        {
            var beeTypes = await _ahwdLookupService.GetAllBeeTypesAsync();
            if (beeTypes is null) return;

            var tempData = _tempDataBeeSpecies.GetAllBeeSpecies();
            BeeType.Clear();
            ErrBeeTypeBool = false;

            foreach (var item in beeTypes)
            {
                if (tempData!.Any(x =>
                    x!.BeeTypeId == item.BeeTypeId &&
                    x.Gmicntry == SelectedCountry?.Gmicntry))
                    continue;
                BeeType.Add(item!);
            }

            ShowBeeType = BeeType.Any();
        }


        // Add Bee Species Command
        [RelayCommand]
        private async Task AddBeeSpeciesAsync()
        {
            try
            {
                ErrNumberOfColoniesBool = NumberOfColonies == 0;
                ErrBeeTypeBool = SelectedBeeType == null;
                ErrBsColoniesBool = SelectedBsColonies == null;
                ErrProvinceSourceBool = SelectedImported == "No" &&
                                        SelectedProvinceSource == null;
                ErrCountryBool = SelectedImported == "Yes" &&
                                  SelectedCountry == null;

                if (ErrNumberOfColoniesBool || ErrBeeTypeBool ||
                    ErrBsColoniesBool || ErrProvinceSourceBool ||
                    ErrCountryBool) return;

                if (string.IsNullOrEmpty(_locationId)) return;

                using (await _loading.Show())
                {
                    _tempDataBeeSpecies.AddBeeSpecies(new TempDataBeeSpeciesModel
                    {
                        LocationId = _locationId,
                        BeeTypeId = SelectedBeeType!.BeeTypeId,
                        BeeTypeDescription = SelectedBeeType.BeeTypeDescription,
                        NumberColonies = NumberOfColonies,
                        Bscolonies = SelectedBsColonies!.Bscolonies,
                        BscoloniesDescription = SelectedBsColonies.BscoloniesDescription!,
                        ProvCode = SelectedProvinceSource?.ProvCode,
                        ProvinceName = SelectedProvinceSource?.ProvinceName,
                        IfImported = SelectedImported == "Yes",
                        Gmicntry = SelectedCountry?.Gmicntry,
                        CountryName = SelectedCountry?.CountryName
                    });

                    SelectedBeeType = new();
                    SelectedBsColonies = new();
                    SelectedProvinceSource = new();
                    SelectedCountry = new();
                    NumberOfColonies = 0;
                    BeeType.Clear();
                    ShowBeeType = false;
                    ErrNumberOfColoniesBool = ErrBsColoniesBool =
                    ErrBeeTypeBool = ErrProvinceSourceBool =
                    ErrCountryBool = false;

                    _popupService.ShowSuccessDialog("Bee Species added successfully");
                    _ = LoaderAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Property Changed Handlers
        partial void OnNumberOfColoniesChanged(int newValue)
        {
            ErrNumberOfColoniesBool = newValue == 0;
        }
    }
}
