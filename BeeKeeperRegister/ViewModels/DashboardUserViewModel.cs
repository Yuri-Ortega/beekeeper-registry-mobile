using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Models.UI;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using BeeKeeperRegister.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;
using BeeKeeperRegister.Models.Response;


namespace BeeKeeperRegister.ViewModels
{
    public partial class DashboardUserViewModel : ObservableObject
    {
        private readonly IAccountService _accountService;
        private readonly IBeeKeeperTrainingService _trainingsService;
        private readonly IBeeKeeperRegistrationService _registerService;
        private readonly IBeeKeeperFarmProfileService _farmProfileService;
        private readonly IBeeLocationProductionTypeSourceService _productionTypeService;
        private readonly IBeeProductioonService _beeProductioonService;
        private readonly IBeeLocationForageService _forageService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly TempDataBeeSpecies _tempDataBeeSpecies;
        private readonly TempDataBeeProductioon _tempDataBeeProductioon;


        // Training Properties
        [ObservableProperty]
        private ObservableCollection<TrainingUIModel> trainingList = new();

        [ObservableProperty]
        private string? filterTrainingsString;

        [ObservableProperty]
        private int? countTrainings = 0;

        [ObservableProperty]
        private bool trainingListIsEmpty = true;

        [ObservableProperty]
        private bool trainingListHasData;

        [ObservableProperty]
        private bool noSearchResultsForTrainingList;

        [ObservableProperty]
        private bool isRefreshingTrainingList = false;

        private string _currentSearchTextForTrainingList = string.Empty;


        // Farm Properties
        [ObservableProperty]
        private ObservableCollection<FarmUIModel> farmList = new();

        [ObservableProperty]
        private int? countFarms = 0;

        [ObservableProperty]
        private int totalColonies; 

        [ObservableProperty]
        private int totalProductioonKG;

        [ObservableProperty]
        private bool farmListIsEmpty = true;

        [ObservableProperty]
        private bool farmListHasData;

        [ObservableProperty]
        private bool noSearchResultsForFarmList;

        [ObservableProperty]
        private bool isRefreshingFarmList = false;

        private string _currentSearchTextForFarmList = string.Empty;

        //Profile Properties
        [ObservableProperty]
        private ImageSource? selectedImageSource;
        [ObservableProperty]
        private string fullName = string.Empty;
        [ObservableProperty]
        private string email = string.Empty;
        [ObservableProperty]
        private string contactNumber = string.Empty;
        [ObservableProperty]
        private string? birthday;
        [ObservableProperty]
        private string sex = string.Empty;

        // Summary Properties
        [ObservableProperty]
        private string numberOfFarm = string.Empty;

        [ObservableProperty]
        private ObservableCollection<SummaryChartItemResponseModel> speciesChartData = new();

        [ObservableProperty]
        private ObservableCollection<SummaryChartItemResponseModel> productionChartData = new();

        [ObservableProperty]
        private ObservableCollection<SummaryChartItemResponseModel> foragesChartData = new();

        [ObservableProperty]
        private bool hasNoSpeciesChartData;

        [ObservableProperty]
        private bool hasNoProductioonChartData;

        [ObservableProperty]
        private bool hasNoForagesChartData;

        [ObservableProperty]
        private double foragesChartWidth = 400;


        public DashboardUserViewModel(
            IAccountService accountService,
            IBeeKeeperTrainingService trainingsService,
            IBeeKeeperRegistrationService registerService,
            IBeeKeeperFarmProfileService farmProfileService,
            IBeeLocationProductionTypeSourceService productionTypeService,
            IBeeProductioonService beeProductioonService,
            IBeeLocationForageService forageService,
            ILoadingPopupService loading,
            IDialogPopupService popupService,
            TempDataBeeSpecies tempDataBeeSpecies,
            TempDataBeeProductioon tempDataBeeProductioon)
        {
            _accountService = accountService;
            _trainingsService = trainingsService;
            _registerService = registerService;
            _farmProfileService = farmProfileService;
            _productionTypeService = productionTypeService;
            _beeProductioonService = beeProductioonService;
            _forageService = forageService;
            _loading = loading;
            _popupService = popupService;
            _tempDataBeeSpecies = tempDataBeeSpecies;
            _tempDataBeeProductioon = tempDataBeeProductioon;
        }


        // Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            _tempDataBeeSpecies.ClearAll();
            _tempDataBeeProductioon.ClearAll();

            await Task.WhenAll(
                LoadTrainingsAsync(),
                LoadFarmProfileAsync(),
                LoadProfileAsync(),
                LoadCountFarmProfile(),
                LoadSummaryAsync()
            );
        }

        [RelayCommand]
        public async Task PullToRefreshTrainingListAsync()
        {
            await Task.Delay(1000);
            await LoadTrainingsAsync();
            IsRefreshingTrainingList = false;
        }

        private async Task LoadTrainingsAsync()
        {
            var trainings = await _trainingsService.GetAllBeeKeeperTrainingsAsync();
            if (trainings is null) return;

            TrainingList.Clear();
            foreach (var item in trainings)
                TrainingList.Add(new TrainingUIModel
                {
                    TrainingCtr = item.TrainingCtr,
                    TrainingDescription = item.TrainingDescription,
                    TrainingYear = item.TrainingYear ?? 0,
                    TrainingHour = item.NumberofHrs ?? 0
                });

            if (TrainingList.Any())
                CountTrainings = TrainingList.Count();

            _currentSearchTextForTrainingList = string.Empty;
            NoSearchResultsForTrainingList = false;
            TrainingListIsEmpty = !trainings.Any();
            TrainingListHasData = trainings.Any();
        }

        [RelayCommand]
        public async Task PullToRefreshFarmListAsync()
        {
            await Task.Delay(1000);
            await LoadFarmProfileAsync();
            IsRefreshingFarmList = false;
        }

        private async Task LoadFarmProfileAsync()
        {
            var farms = await _farmProfileService.GetAllFarmProfilesAsync();
            if (farms is null) return;

            FarmList.Clear();
            foreach (var farm in farms)
            {
                var locationId = $"{farm.Bcode}{farm.LotNo}";

                var speciesTask = _productionTypeService
                    .CountBeeSpeciesPerFarmByLocationIdAsync(locationId);
                var coloniesTask = _productionTypeService
                    .CountBeeColoniesPerFarmByLocationIdAsync(locationId);
                var forageTask = _forageService
                    .CountBeeLocationForagesPerFarmByLocationIdAsync(locationId);
                var productioonTask = _beeProductioonService.CountBeeProductioonPerFarmByLocationIdAsync(locationId);

                await Task.WhenAll(speciesTask, coloniesTask, productioonTask, forageTask);

                FarmList.Add(new FarmUIModel
                {
                    LocationId = locationId,
                    Location = $"{farm.Barangay}, {farm.Municipalities}, {farm.Provinces}",
                    LotNo = farm.LotNo,
                    NumberSpecies = await speciesTask,
                    NumberColonies = await coloniesTask,
                    NumberForages = await forageTask,
                    NumberProductioon = await productioonTask,
                    Latitude = Convert.ToDouble(farm.Latitude),
                    Longitude = Convert.ToDouble(farm.Longitude),
                });
            }

            if (FarmList.Any())
                CountFarms = FarmList.Count();

            _currentSearchTextForFarmList = string.Empty;
            NoSearchResultsForFarmList = false;
            FarmListIsEmpty = !farms.Any();
            FarmListHasData = farms.Any();
        }


        private async Task LoadProfileAsync()
        {
            var userData = await _accountService.GetUserProfileAsync();
            if (userData is null) return;

            var result = await _accountService.GetProfileImageAsync();

            if (result?.ProfilePicture is not null && result.ProfilePicture.Length > 0)
                SelectedImageSource = ImageSource.FromStream(
                        () => new MemoryStream(result.ProfilePicture));
            else
                SelectedImageSource = ImageSource.FromFile("default_user.jpg");


            FullName = $"{userData.Firstname} {userData.Lastname}";
            Email = userData.Email!;
            ContactNumber = userData.PhoneNumber!;
            Birthday = userData.Birthday.ToString();
            Sex = userData.SexDescription!;
        }

        private async Task LoadCountFarmProfile()
        {
            var farmCount = await _farmProfileService.CountFarmProfilesAsync();
            if (farmCount is null) return;
            NumberOfFarm = farmCount.ToString()!;
        }

        private async Task LoadSummaryAsync()
        {
            HasNoSpeciesChartData = true;
            HasNoProductioonChartData = true;
            HasNoForagesChartData = true;
            var productionTask = _productionTypeService.GetAllBeeLocationProductionTypeSourcesAsync();
            var beeProductioonTask = _beeProductioonService.GetAllBeeProductioonAsync();
            var forageTask = _forageService.GetAllBeeLocationForagesAsync();

            await Task.WhenAll(productionTask, beeProductioonTask, forageTask);

            var production = await productionTask;
            var beeProductioon = await beeProductioonTask;
            var forages = await forageTask;

            if (production is not null)
            {
                TotalColonies = production.Sum(u => u?.NumberColonies ?? 0);
                
                SpeciesChartData.Clear();
                var speciesGroups = production
                    .Where(u => !string.IsNullOrEmpty(u?.BeeTypeDescription))
                    .GroupBy(u => FilterHandler.RemoveSpacingBeeSummary(u!.BeeTypeDescription));

                if(speciesGroups.Any())
                    HasNoSpeciesChartData = false;

                foreach (var g in speciesGroups)
                    SpeciesChartData.Add(new SummaryChartItemResponseModel { Label = g.Key!, Count = g.Count() });
            }

            if (beeProductioon is not null)
            {
                TotalProductioonKG = beeProductioon.Sum(u => u?.EstProdYield ?? 0);
                
                ProductionChartData.Clear();
                var productionGroups = beeProductioon
                    .Where(u => !string.IsNullOrEmpty(u?.BeeProductionDescription))
                    .GroupBy(u => FilterHandler.RemoveSpacingBeeSummary(u!.BeeProductionDescription));

                if (productionGroups.Any())
                    HasNoProductioonChartData = false;

                foreach (var g in productionGroups)
                    ProductionChartData.Add(new SummaryChartItemResponseModel { Label = g.Key!, Count = g.Count() });
            }

            if (forages is not null)
            {
                
                ForagesChartData.Clear();
                var foragesGroups = forages
                    .Where(u => !string.IsNullOrEmpty(u?.ForagesDescription))
                    .GroupBy(u => FilterHandler.RemoveSpacingBeeSummary(u!.ForagesDescription!));

                if (foragesGroups.Any())
                    HasNoForagesChartData = false;

                foreach (var g in foragesGroups)
                    ForagesChartData.Add(new SummaryChartItemResponseModel { Label = g.Key!, Count = g.Count() });

                RecalculateForagesChartWidth();
            }
        }

        public Color[] BrandChartPalette { get; } = new Color[]
        {
            Color.FromArgb("#5C6B2F"), // olive
            Color.FromArgb("#C9962F"), // gold
            Color.FromArgb("#9CA052"), // light olive
            Color.FromArgb("#B5651D"), // warm accent
            Color.FromArgb("#2F6B5C"), // cool complement
            Color.FromArgb("#7A5C3E"), // earthy brown
        };

        private void RecalculateForagesChartWidth()
        {
            const double minWidthPerBar = 70;
            const double minTotalWidth = 400;
            ForagesChartWidth = Math.Max(minTotalWidth, ForagesChartData.Count * minWidthPerBar);
        }

        // Training Commands
        [RelayCommand]
        private void SearchTrainings(string text)
        {
            _currentSearchTextForTrainingList = text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(_currentSearchTextForTrainingList))
            {
                FilterTrainingsString = null;
                NoSearchResultsForTrainingList = false;

                TrainingListIsEmpty = !TrainingList.Any();
                TrainingListHasData = TrainingList.Any();
            }
            else
            {
                FilterTrainingsString =
                    $"Contains([TrainingDescription], '{text}') or " +
                    $"Contains([TrainingYear], '{text}') or " +
                    $"Contains([TrainingHour], '{text}')";

                var hasResults = TrainingList.Any(t =>
                    (t.TrainingDescription?.Contains(text!, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    t.TrainingYear.ToString()!.Contains(text!) ||
                    t.TrainingHour.ToString()!.Contains(text!));

                NoSearchResultsForTrainingList = !hasResults;
                TrainingListHasData = hasResults;
                TrainingListIsEmpty = false;
            }
        }

        [RelayCommand]
        public async Task AddTrainingAsync() =>
            await Shell.Current.GoToAsync(nameof(AddTrainingsPage));

        [RelayCommand]
        public async Task UpdateAsync(TrainingUIModel item)
        {
            if (item is null) return;
            await Shell.Current.GoToAsync(nameof(UpdateTrainingsPage),
                new Dictionary<string, object>
                {
                    { "TrainingCtr", item.TrainingCtr }
                });
        }

        [RelayCommand]
        public async Task DeleteAsync(TrainingUIModel item)
        {
            bool isYes = await _popupService
                .ShowConfirmDialog("Delete", "Are you sure you want to delete?", "Yes", "No");
            if (!isYes) return;

            using (await _loading.Show())
            {
                var isRemoved = await _trainingsService.DeleteTrainingByTrainingCtrAsync(item.TrainingCtr);
                if (isRemoved)
                    _popupService.ShowSuccessDialog(
                        $"{item.TrainingDescription} deleted successfully");

                _ = LoaderAsync();
            }
        }

        [RelayCommand]
        public async Task ViewDetailsAsync(FarmUIModel item)
        {
            if (item is null) return;
            await Shell.Current.GoToAsync(nameof(ViewFarmProfilePage),
                new Dictionary<string, object>
                {
                    { "LocationId", item.LocationId }
                });
        }

        [RelayCommand]
        public async Task AddFarmLocationProfileAsync() =>
            await Shell.Current.GoToAsync(nameof(AddFarmProfilePage));


        [RelayCommand]
        public async Task EditProfileImageAsync() =>
            await Shell.Current.GoToAsync(nameof(EditProfileImagePage));

        // Navigation Settings Commands 
        [RelayCommand]
        public async Task SettingsAsync() =>
            await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}
