using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models;
using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class UpdateBeeProductioonViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeProductioonService _beeProductioonService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;

        //Bee Production
        [ObservableProperty] private string beeProdID = string.Empty;
        [ObservableProperty] private string locationId = string.Empty;
        [ObservableProperty] private int? estProdYield;
        [ObservableProperty] private bool errBeeProductionBool;
        [ObservableProperty] private bool errEstProdYieldBool;

        //Selected Item
        [ObservableProperty] private BeeProductionResponseModel? selectedBeeProduction;

        //Collections
        [ObservableProperty]
        private ObservableCollection<BeeProductionResponseModel> beeProduction = new();

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("BeeProdID", out var beeProdIDValue))
                BeeProdID = beeProdIDValue?.ToString() ?? string.Empty;

            if (query.TryGetValue("LocationId", out var locationIdValue))
                LocationId = locationIdValue?.ToString() ?? string.Empty;
        }

        public UpdateBeeProductioonViewModel(
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
            var categoriesTask = _ahwdLookupService.GetAllBeeProductionCategoriesAsync();
            var currentTask = _beeProductioonService.GetBeeProductioonByBeeProdIdAsync(BeeProdID);
            var existingTask = _beeProductioonService.GetAllBeeProductioonByLocationIdAsync(LocationId);

            await Task.WhenAll(categoriesTask, currentTask, existingTask);

            var categories = await categoriesTask;
            var current = await currentTask;
            var existing = await existingTask;

            if (categories is null || current is null) return;

            BeeProduction.Clear();
            ErrBeeProductionBool = false;

            foreach (var item in categories)
            {
                bool isDuplicate = existing?.Any(x =>
                    x?.BeeProdId == item.BeeProdId &&
                    x.BeeProdId != current.BeeProdId) == true;

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

        //update Bee Production Command
        [RelayCommand]
        private async Task UpdateBeeProductionAsync()
        {
            try
            {
                ErrBeeProductionBool = SelectedBeeProduction == null;
                ErrEstProdYieldBool = EstProdYield == 0;
                if (ErrBeeProductionBool || ErrEstProdYieldBool) return;
                if (string.IsNullOrEmpty(BeeProdID)) return;

                using (await _loading.Show())
                {
                    var isUpdated = await _beeProductioonService.UpdateBeeProductioonAsync(
                        new UpdateProductioonRequestModel
                        {
                            LocationId = LocationId,
                            OldBeeProdId = BeeProdID,
                            BeeProdId = SelectedBeeProduction!.BeeProdId,
                            BeeProductionDescription = SelectedBeeProduction.BeeProductionDescription,
                            EstProdYield = EstProdYield ?? 0
                        });

                    if (!isUpdated) return;

                    _popupService.ShowSuccessDialog("Bee Production updated successfully");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        //Property Changed Handler
        partial void OnEstProdYieldChanged(int? value)
        {
            ErrEstProdYieldBool = value == 0;
        }
    }
}
