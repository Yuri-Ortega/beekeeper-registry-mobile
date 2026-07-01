using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class AddBeeSpeciesViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeLocationProductionTypeSourceService _productionTypeService;
        private readonly IOneBAILookupService _onebaiLookupService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;

        //BeeSpecies Fields
        [ObservableProperty] private string locationId = string.Empty;
        [ObservableProperty] private int numberOfColonies;
        [ObservableProperty] private string selectedImported = "No";
        [ObservableProperty] private bool isLocal = true;
        [ObservableProperty] private bool isImported;
        [ObservableProperty] private bool showBeeType;
        [ObservableProperty] private bool errNumberOfColoniesBool;
        [ObservableProperty] private bool errProvinceSourceBool;
        [ObservableProperty] private bool errCountryBool;
        [ObservableProperty] private bool errBeeTypeBool;
        [ObservableProperty] private bool errBsColoniesBool;

        //Selected Items
        [ObservableProperty] private BeeTypesModel? selectedBeeType;
        [ObservableProperty] private BeeSourceColoniesModel? selectedBsColonies;
        [ObservableProperty] private ProvinceModel? selectedProvinceSource;
        [ObservableProperty] private CountryModel? selectedCountry;


        //Collections
        [ObservableProperty]
        private ObservableCollection<BeeTypesModel> beeType = new();

        [ObservableProperty]
        private ObservableCollection<BeeSourceColoniesModel> bsColonies = new();

        [ObservableProperty]
        private ObservableCollection<ProvinceModel> provinceSource = new();

        [ObservableProperty]
        private ObservableCollection<CountryModel> country = new();

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("LocationId", out var value))
                LocationId = value?.ToString() ?? string.Empty;
        }

        public AddBeeSpeciesViewModel(
            IBeeLocationProductionTypeSourceService productionTypeService,
            IOneBAILookupService onebaiLookupService,
            ILoadingPopupService loading,
            IDialogPopupService popupService,
            IAHWDLookupService ahwdLookupService)
        {
            _productionTypeService = productionTypeService;
            _onebaiLookupService = onebaiLookupService;
            _loading = loading;
            _popupService = popupService;
            _ahwdLookupService = ahwdLookupService;
        }

        //Loader
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
            var data = await _onebaiLookupService.GetProvincesAsync();
            if (data is null) return;
            ProvinceSource.Clear();
            ErrProvinceSourceBool = false;
            foreach (var item in data) ProvinceSource.Add(item!);
        }

        private async Task LoadCountriesAsync()
        {
            var data = await _onebaiLookupService.GetCountriesAsync();
            if (data is null) return;
            Country.Clear();
            ErrCountryBool = false;
            foreach (var item in data) Country.Add(item!);
        }

        //Property Changed Handler
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

        partial void OnNumberOfColoniesChanged(int newValue)
        {
            ErrNumberOfColoniesBool = newValue == 0;
        }

        //Selection Event
        [RelayCommand]
        public async Task SelectionProvinceSourceAsync()
        {
            if (SelectedProvinceSource == null) return;
            await LoadBeeTypesByLocationAsync(
                provCode: SelectedProvinceSource.ProvCode);
            ErrProvinceSourceBool = false;
        }

        [RelayCommand]
        public async Task SelectionCountryAsync()
        {
            if (SelectedCountry == null) return;
            await LoadBeeTypesByLocationAsync(
                gmicntry: SelectedCountry.Gmicntry);
            ErrCountryBool = false;
        }

        private async Task LoadBeeTypesByLocationAsync(
            string? provCode = null,
            string? gmicntry = null)
        {
            var beeTypes = await _ahwdLookupService.GetAllBeeTypesAsync();
            if (beeTypes is null) return;

            var existing = await _productionTypeService
                .GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(LocationId);

            BeeType.Clear();
            ErrBeeTypeBool = false;

            foreach (var item in beeTypes)
            {
                bool isDuplicate = existing?.Any(x =>
                    x?.BeeTypeId == item.BeeTypeId &&
                    (provCode != null
                        ? x?.ProvCode == provCode
                        : x?.Gmicntry == gmicntry)) == true;

                if (isDuplicate) continue;
                BeeType.Add(item!);
            }

            ShowBeeType = BeeType.Any();
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

        //Add Bee Species Command
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

                if (string.IsNullOrEmpty(LocationId)) return;

                using (await _loading.Show())
                {
                    var isCreated = await _productionTypeService.AddBeeLocationProductionTypeSourceAsync(
                        new AddBeeLocationProductionTypeSourceModel
                        {
                            LocationId = LocationId,
                            BeeTypeId = SelectedBeeType!.BeeTypeId,
                            BeeTypeDescription = SelectedBeeType.BeeTypeDescription,
                            NumberColonies = NumberOfColonies,
                            Bscolonies = SelectedBsColonies!.Bscolonies,
                            BscoloniesDescription = SelectedBsColonies.BscoloniesDescription!,
                            ProvCode = SelectedProvinceSource?.ProvCode ?? string.Empty,
                            ProvinceName = SelectedProvinceSource?.ProvinceName ?? string.Empty,
                            IfImported = SelectedImported == "Yes",
                            Gmicntry = SelectedCountry?.Gmicntry ?? string.Empty,
                            CountryName = SelectedCountry?.CountryName ?? string.Empty
                        });

                    if (!isCreated) return;

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

    }
}
