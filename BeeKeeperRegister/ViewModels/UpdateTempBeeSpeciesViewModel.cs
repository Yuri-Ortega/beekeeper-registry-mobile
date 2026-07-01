using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class UpdateTempBeeSpeciesViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeLocationProductionTypeSourceService _productionTypeService;
        private readonly IOneBAILookupService _OneBAILookupService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;
        private readonly TempDataBeeSpecies _tempDataBeeSpecies;

        //Query
        private int _id;
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Id", out var value))
                _id = Convert.ToInt32(value);
        }


        // Bee Species Fields
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

        //Selected Event
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

        public UpdateTempBeeSpeciesViewModel(
            IBeeLocationProductionTypeSourceService productionTypeService,
            IOneBAILookupService lookupService,
            ILoadingPopupService loading,
            TempDataBeeSpecies tempDataBeeSpecies,
            IDialogPopupService popupService,
            IAHWDLookupService ahwdLookupService)
        {
            _productionTypeService = productionTypeService;
            _OneBAILookupService = lookupService;
            _tempDataBeeSpecies = tempDataBeeSpecies;
            _loading = loading;
            _popupService = popupService;
            _ahwdLookupService = ahwdLookupService;
        }


        // Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var beeTypesTask = _ahwdLookupService.GetAllBeeTypesAsync();
            var bsColoniesTask = _ahwdLookupService.GetAllBeeSourcesColoniesAsync();
            var provincesTask = _OneBAILookupService.GetProvincesAsync();
            var countriesTask = _OneBAILookupService.GetCountriesAsync();

            await Task.WhenAll(
                beeTypesTask, bsColoniesTask,
                provincesTask, countriesTask);

            var beeTypes = await beeTypesTask;
            var bsColoniesData = await bsColoniesTask;
            var provinces = await provincesTask;
            var countries = await countriesTask;

            var current = _tempDataBeeSpecies.GetByID(_id);
            var tempData = _tempDataBeeSpecies.GetAllBeeSpecies();

            if (beeTypes is null || current is null ||
                bsColoniesData is null || provinces is null ||
                countries is null) return;

            BeeType.Clear();
            ErrBeeTypeBool = false;
            foreach (var item in beeTypes)
            {
                bool isDuplicate = tempData.Any(x =>
                    x.BeeTypeId == item.BeeTypeId &&
                    (x.ProvCode == current.ProvCode ||
                     x.Gmicntry == current.Gmicntry));

                bool isCurrent = item.BeeTypeId == current.BeeTypeId;
                if (isDuplicate && !isCurrent) continue;
                BeeType.Add(item!);
            }

            //BsColonies
            BsColonies.Clear();
            ErrBsColoniesBool = false;
            foreach (var item in bsColoniesData) BsColonies.Add(item!);

            //Provinces
            ProvinceSource.Clear();
            ErrProvinceSourceBool = false;
            foreach (var item in provinces) ProvinceSource.Add(item!);

            //Countries
            Country.Clear();
            ErrCountryBool = false;
            foreach (var item in countries) Country.Add(item!);

            //Set current values
            SelectedBeeType = BeeType
                .FirstOrDefault(x => x.BeeTypeId == current.BeeTypeId);
            NumberOfColonies = current.NumberColonies;
            SelectedBsColonies = BsColonies
                .FirstOrDefault(x => x.Bscolonies == current.Bscolonies);
            SelectedProvinceSource = ProvinceSource
                .FirstOrDefault(x => x.ProvinceName == current.ProvinceName);
            SelectedImported = current.IfImported == true ? "Yes" : "No";
            SelectedCountry = Country
                .FirstOrDefault(x => x.CountryName == current.CountryName);
        }


        // Update Bee Species Command
        [RelayCommand]
        private async Task UpdateBeeSpeciesAsync()
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

                if (_id == 0) return;

                using (await _loading.Show())
                {
                    _tempDataBeeSpecies.UpdateBeeSpecies(
                        new TempDataBeeSpeciesModel
                        {
                            Id = _id,
                            BeeTypeId = SelectedBeeType!.BeeTypeId,
                            BeeTypeDescription = SelectedBeeType.BeeTypeDescription,
                            NumberColonies = NumberOfColonies,
                            Bscolonies = SelectedBsColonies!.Bscolonies,
                            BscoloniesDescription =
                                SelectedBsColonies.BscoloniesDescription!,
                            ProvCode = SelectedProvinceSource?.ProvCode,
                            ProvinceName = SelectedProvinceSource?.ProvinceName,
                            IfImported = SelectedImported == "Yes",
                            Gmicntry = SelectedCountry?.Gmicntry,
                            CountryName = SelectedCountry?.CountryName
                        });

                    _popupService.ShowSuccessDialog(
                        "Bee Species updated successfully");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Selection Events
        partial void OnSelectedImportedChanged(string value)
        {
            if (value == "Yes")
            {
                SelectedProvinceSource = null;
                IsImported = true;
                IsLocal = false;
            }
            else
            {
                SelectedCountry = null;
                IsImported = false;
                IsLocal = true;
            }
        }

        [RelayCommand]
        public async Task SelectionProvinceSourceAsync()
        {
            if (SelectedProvinceSource == null) return;
            await LoadFilteredBeeTypesAsync(
                provCode: SelectedProvinceSource.ProvCode);
            ErrProvinceSourceBool = false;
        }

        [RelayCommand]
        public async Task SelectionCountryAsync()
        {
            if (SelectedCountry == null) return;
            await LoadFilteredBeeTypesAsync(
                gmicntry: SelectedCountry.Gmicntry);
            ErrCountryBool = false;
        }

        private async Task LoadFilteredBeeTypesAsync(
            string? provCode = null,
            string? gmicntry = null)
        {
            var beeTypes = await _ahwdLookupService.GetAllBeeTypesAsync();
            if (beeTypes is null) return;

            var current = _tempDataBeeSpecies.GetByID(_id);
            var tempData = _tempDataBeeSpecies.GetAllBeeSpecies();

            if (current is null) return;

            var filtered = FilterHandler.TempFilterBeeTypes(
                beeTypes, tempData,
                current.BeeTypeId, provCode, gmicntry);

            BeeType.Clear();
            ErrBeeTypeBool = false;
            foreach (var item in filtered) BeeType.Add(item);
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

        //Property Changed Handler
        partial void OnNumberOfColoniesChanged(int newValue)
        {
            ErrNumberOfColoniesBool = newValue == 0;
        }
    }
}
