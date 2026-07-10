using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class AddTrainingsViewModel : ObservableObject
    {
        private readonly IBeeKeeperTrainingService _trainingsService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;

        // Training year message color 
        private static readonly Color ErrorColor = Color.FromArgb("#D32F2F");
        private static readonly Color NormalColor = Color.FromArgb("#030303");

        //Training Fields
        [ObservableProperty] private int trainingYear;
        [ObservableProperty] private int numberOfHrs;
        [ObservableProperty] private bool errNumberOfHrsBool;
        [ObservableProperty] private bool errBeeTrainingsBool;
        [ObservableProperty] private Color errTrainingYearColor = Color.FromArgb("#030303");

        //Selected Items
        [ObservableProperty] private BeeTrainingResponseModel? selectedBeeTrainings;

        //Collection 7 List
        [ObservableProperty]
        private ObservableCollection<BeeTrainingResponseModel> beeTrainings = new();

        public List<int> Years { get; } =
            Enumerable.Range(1900, DateTime.Now.Year - 1900 + 1).ToList();

        public AddTrainingsViewModel(
            IBeeKeeperTrainingService trainingsService,
            ILoadingPopupService loading,
            IDialogPopupService popupService,
            IAHWDLookupService ahwdLookupService)
        {
            _trainingsService = trainingsService;
            _loading = loading;
            _popupService = popupService;
            _ahwdLookupService = ahwdLookupService;
        }

        //Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var allTrainings = await _ahwdLookupService.GetAllBeeTrainingsAsync();
            if (allTrainings is null) return;

            var existing = await _trainingsService.GetAllBeeKeeperTrainingsAsync();

            BeeTrainings.Clear();
            ErrBeeTrainingsBool = false;

            foreach (var item in allTrainings)
            {
                if (existing?.Any(x => x.TrainingCode == item.TrainingCode) == true)
                    continue;
                BeeTrainings.Add(item!);
            }
        }

        //Selection Event
        [RelayCommand]
        public void SelectionBeeTrainings()
        {
            if (SelectedBeeTrainings == null) return;
            ErrBeeTrainingsBool = false;
        }

        //Add Training Command
        [RelayCommand]
        private async Task AddTrainingAsync()
        {
            try
            {
                ErrTrainingYearColor = TrainingYear == 0 ? ErrorColor : NormalColor;
                ErrNumberOfHrsBool = NumberOfHrs == 0;
                ErrBeeTrainingsBool = SelectedBeeTrainings == null;

                if (ErrTrainingYearColor == ErrorColor ||
                    ErrNumberOfHrsBool || ErrBeeTrainingsBool) return;

                using (await _loading.Show())
                {
                    var isCreated = await _trainingsService.AddTrainingAsync(
                        new AddBeeKeeperTrainingsRequestModel
                        {
                            TrainingCode = SelectedBeeTrainings!.TrainingCode,
                            TrainingDescription = SelectedBeeTrainings.TrainingDescription!,
                            TrainingYear = TrainingYear,
                            NumberofHrs = NumberOfHrs
                        });

                    if (!isCreated) return;

                    SelectedBeeTrainings = new();
                    NumberOfHrs = TrainingYear = 0;
                    ErrNumberOfHrsBool = ErrBeeTrainingsBool = false;
                    ErrTrainingYearColor = NormalColor;

                    _popupService.ShowSuccessDialog("Training added successfully");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Property Changed Handlers
        partial void OnTrainingYearChanged(int value)
        {
            ErrTrainingYearColor = value == 0 ? ErrorColor : NormalColor;
        }

        partial void OnNumberOfHrsChanged(int value)
        {
            ErrNumberOfHrsBool = value == 0;
        }
    }
}
