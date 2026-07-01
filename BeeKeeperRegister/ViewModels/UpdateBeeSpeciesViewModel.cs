using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class UpdateBeeSpeciesViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeLocationProductionTypeSourceService _productionTypeService;
        private readonly IOneBAILookupService _oneBAILookupService;
        private readonly IAHWDLookupService _ahwdLookupService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;

        //Bee Species Fields
        [ObservableProperty] private int beeProdCtr;
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
            if (query.TryGetValue("BeeProdCtr", out var ctr))
                BeeProdCtr = Convert.ToInt32(ctr);

            if (query.TryGetValue("LocationId", out var loc))
                LocationId = loc?.ToString() ?? string.Empty;
        }

        public UpdateBeeSpeciesViewModel(
            IBeeLocationProductionTypeSourceService productionTypeService,
            IOneBAILookupService oneBAILookupService,
            IAHWDLookupService ahwdLookupService,
            ILoadingPopupService loading,
            IDialogPopupService popupService)
        {
            _productionTypeService = productionTypeService;
            _oneBAILookupService = oneBAILookupService;
            _ahwdLookupService = ahwdLookupService;
            _loading = loading;
            _popupService = popupService;
        }

        //loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var beeTypesTask = _ahwdLookupService.GetAllBeeTypesAsync();
            var currentTask = _productionTypeService.GetBeeLocationProductionTypeSourceByBeeProdCtrAsync(BeeProdCtr);
            var existingTask = _productionTypeService.GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(LocationId);
            var bsColoniesTask = _ahwdLookupService.GetAllBeeSourcesColoniesAsync();
            var provincesTask = _oneBAILookupService.GetProvincesAsync();
            var countriesTask = _oneBAILookupService.GetCountriesAsync();

            await Task.WhenAll(
                beeTypesTask, currentTask, existingTask,
                bsColoniesTask, provincesTask, countriesTask);

            var beeTypes = await beeTypesTask;
            var current = await currentTask;
            var existing = await existingTask;
            var bsColoniesData = await bsColoniesTask;
            var provincesData = await provincesTask;
            var countriesData = await countriesTask;

            if (beeTypes is null || current is null) return;

            //BeeType filter
            BeeType.Clear();
            ErrBeeTypeBool = false;
            foreach (var item in beeTypes)
            {
                bool isDuplicate = existing?.Any(x =>
                    x?.BeeTypeId == item.BeeTypeId &&
                    (x.ProvCode == current.ProvCode ||
                     x.Gmicntry == current.Gmicntry)) == true;

                bool isCurrent = item.BeeTypeId == current.BeeTypeId;

                if (isDuplicate && !isCurrent) continue;
                BeeType.Add(item);
            }

            //BsColonies
            BsColonies.Clear();
            ErrBsColoniesBool = false;
            if (bsColoniesData is not null)
                foreach (var item in bsColoniesData) BsColonies.Add(item!);

            //ProvinceSource
            ProvinceSource.Clear();
            ErrProvinceSourceBool = false;
            if (provincesData is not null)
                foreach (var item in provincesData) ProvinceSource.Add(item!);

            //Country
            Country.Clear();
            ErrCountryBool = false;
            if (countriesData is not null)
                foreach (var item in countriesData) Country.Add(item!);

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

        //Update Bee Species Command
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

                if (BeeProdCtr == 0) return;

                using (await _loading.Show())
                {
                    var isUpdated = await _productionTypeService.UpdateBeeLocationProductionTypeSourceAsync(
                        new UpdateBeeLocationProductionTypeSourceModel
                        {
                            BeeProdCtr = BeeProdCtr,
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

                    if (!isUpdated) return;

                    _popupService.ShowSuccessDialog("Bee Species updated successfully");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        //Property Changed Handler
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

        partial void OnNumberOfColoniesChanged(int newValue)
        {
            ErrNumberOfColoniesBool = newValue == 0;
        }

        //Selection Event
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
            var beeTypesTask = _ahwdLookupService.GetAllBeeTypesAsync();
            var currentTask = _productionTypeService.GetBeeLocationProductionTypeSourceByBeeProdCtrAsync(BeeProdCtr);
            var existingTask = _productionTypeService.GetAllBeeLocationProductionTypeSourcesByLocationIdAsync(LocationId);

            await Task.WhenAll(beeTypesTask, currentTask, existingTask);

            var beeTypes = await beeTypesTask;
            var current = await currentTask;
            var existing = await existingTask;

            if (beeTypes is null || current is null || existing is null) return;

            var filtered = FilterHandler.FilterBeeTypes(
                beeTypes, existing, current.BeeTypeId,
                provCode, gmicntry);

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
    }
}
