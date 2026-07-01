using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class AddBeeProductioonViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeProductioonService _beeProductioonService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;

        //Productioon Fields
        [ObservableProperty] private string locationId = string.Empty;
        [ObservableProperty] private int estProdYield;
        [ObservableProperty] private bool errBeeProductionBool;
        [ObservableProperty] private bool errEstProdYieldBool;

        //Selected Items
        [ObservableProperty] private BeeProductionModel? selectedBeeProduction;

        //Collection
        [ObservableProperty]
        private ObservableCollection<BeeProductionModel> beeProduction = new();

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("LocationId", out var value))
                LocationId = value?.ToString() ?? string.Empty;
        }

        public AddBeeProductioonViewModel(
            IBeeProductioonService beeProductioonService,
            ILoadingPopupService loading,
            IDialogPopupService popupService,
            IAHWDLookupService ahwdLookupService)
        {
            _beeProductioonService = beeProductioonService;
            _loading = loading;
            _popupService = popupService;
            _ahwdLookupService = ahwdLookupService; 
        }

        //loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var categories = await _ahwdLookupService.GetAllBeeProductionCategoriesAsync();
            if (categories is null) return;

            var existing = await _beeProductioonService
                .GetAllBeeProductioonByLocationIdAsync(LocationId);

            BeeProduction.Clear();
            ErrBeeProductionBool = false;

            foreach (var item in categories)
            {
                if (existing?.Any(x => x?.BeeProdId == item.BeeProdId) == true)
                    continue;
                BeeProduction.Add(item!);
            }
        }

        //Selection Event
        [RelayCommand]
        public async Task SelectionBeeProductionAsync()
        {
            if (SelectedBeeProduction == null) return;
            ErrBeeProductionBool = false;
        }

        //Add Bee Productioon Command
        [RelayCommand]
        private async Task AddBeeProductioonAsync()
        {
            try
            {
                ErrBeeProductionBool = SelectedBeeProduction == null;
                ErrEstProdYieldBool = EstProdYield == 0;
                if (ErrBeeProductionBool || ErrEstProdYieldBool) return;
                if (string.IsNullOrEmpty(LocationId)) return;

                using (await _loading.Show())
                {
                    var isCreated = await _beeProductioonService.AddBeeProductioonAsync(
                        new BeeProductioonModel
                        {
                            LocationId = LocationId,
                            BeeProdId = SelectedBeeProduction!.BeeProdId,
                            BeeProductionDescription = SelectedBeeProduction.BeeProductionDescription,
                            EstProdYield = EstProdYield
                        });

                    if (!isCreated) return;

                    SelectedBeeProduction = new();
                    EstProdYield = 0;
                    ErrBeeProductionBool = ErrEstProdYieldBool = false;
                    _popupService.ShowSuccessDialog("Bee Production added successfully");
                    _ = LoaderAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Property Changed Handlers
        partial void OnEstProdYieldChanged(int newValue)
        {
            ErrEstProdYieldBool = newValue == 0;
        }
    }
}
