using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Models.Response;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class UpdateTempBeeProductioonViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeProductioonService _beeProductioonService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly TempDataBeeProductioon _tempDataBeeProductioon;
        private readonly IAHWDLookupService _ahwdLookupService;

        //Query
        private int _id;
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Id", out var value))
                _id = Convert.ToInt32(value);
        }

        //Bee Productioon Fields 
        [ObservableProperty] private int estProdYield;
        [ObservableProperty] private bool errBeeProductionBool;
        [ObservableProperty] private bool errEstProdYieldBool;

        //Selected Items
        [ObservableProperty] private BeeProductionResponseModel? selectedBeeProduction;

        //Collections
        [ObservableProperty]
        private ObservableCollection<BeeProductionResponseModel> beeProduction = new();

        public UpdateTempBeeProductioonViewModel(
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

            var current = _tempDataBeeProductioon.GetByID(_id);
            if (current is null) return;

            var tempData = _tempDataBeeProductioon.GetAllBeeProductioon();

            BeeProduction.Clear();
            ErrBeeProductionBool = false;

            foreach (var item in categories)
            {
                bool isDuplicate = tempData.Any(x =>
                    x.BeeProdId == item.BeeProdId &&
                    x.BeeProdId != current.BeeProdId);

                if (isDuplicate) continue;
                BeeProduction.Add(item!);
            }

            SelectedBeeProduction = BeeProduction
                .FirstOrDefault(x => x.BeeProdId == current.BeeProdId);
            EstProdYield = current.EstProdYield;
        }

        //Selection Event
        [RelayCommand]
        public void SelectionBeeProduction()
        {
            if (SelectedBeeProduction == null) return;
            ErrBeeProductionBool = false;
        }

        //update Bee Productioon Command
        [RelayCommand]
        private async Task UpdateBeeProductioonAsync()
        {
            try
            {
                ErrBeeProductionBool = EstProdYield == 0;
                ErrEstProdYieldBool = SelectedBeeProduction == null;
                if (ErrBeeProductionBool || ErrEstProdYieldBool) return;
                if (_id == 0) return;

                using (await _loading.Show())
                {
                    _tempDataBeeProductioon.UpdateBeeProductioon(
                        new TempDataBeeProductioonModel
                        {
                            Id = _id,
                            BeeProdId = SelectedBeeProduction!.BeeProdId,
                            BeeProductionDescription =
                                SelectedBeeProduction.BeeProductionDescription,
                            EstProdYield = EstProdYield
                        });

                    _popupService.ShowSuccessDialog(
                        "Bee Production updated successfully");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        //Property Changed Handler
        partial void OnEstProdYieldChanged(int value)
        {
            ErrEstProdYieldBool = value == 0;
        }
    }
}
