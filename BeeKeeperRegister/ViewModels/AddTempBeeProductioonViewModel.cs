using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class AddTempBeeProductioonViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeProductioonService _beeProductioonService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly TempDataBeeProductioon _tempDataBeeProductioon;
        private readonly IAHWDLookupService _ahwdLookupService;

        private string _locationId = string.Empty;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("LocationId", out var value))
                _locationId = value.ToString()!;
        }

        //Bee Production Fields
        [ObservableProperty]
        private int estProdYield;

        [ObservableProperty]
        private bool errBeeProductionBool;

        [ObservableProperty]
        private bool errEstProdYieldBool;

        ///Selected Items
        [ObservableProperty]
        private BeeProductionModel? selectedBeeProduction;

        //Collections
        [ObservableProperty]
        private ObservableCollection<BeeProductionModel> beeProduction = new();

        public AddTempBeeProductioonViewModel(
            IBeeProductioonService beeProductioonService,
            ILoadingPopupService loading,
            TempDataBeeProductioon tempDataBeeProductioon,
            IDialogPopupService popupService,
            IAHWDLookupService ahwdLookupService)
        {
            _beeProductioonService = beeProductioonService;
            _tempDataBeeProductioon = tempDataBeeProductioon;
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

            var tempData = _tempDataBeeProductioon.GetAllBeeProductioon();

            BeeProduction.Clear();
            ErrBeeProductionBool = false;

            foreach (var item in categories)
            {
                if (tempData.Any(x => x.BeeProdId == item.BeeProdId))
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

                if (string.IsNullOrEmpty(_locationId)) return;

                using (await _loading.Show())
                {
                    _tempDataBeeProductioon.AddBeeProductioon(new TempDataBeeProductioonModel
                    {
                        LocationId = _locationId,
                        BeeProdId = SelectedBeeProduction!.BeeProdId,
                        BeeProductionDescription = SelectedBeeProduction.BeeProductionDescription,
                        EstProdYield = EstProdYield
                    });

                    SelectedBeeProduction = new();
                    EstProdYield = 0;
                    ErrBeeProductionBool = false;
                    ErrEstProdYieldBool = false;

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
