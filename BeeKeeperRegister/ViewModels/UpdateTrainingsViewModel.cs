using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class UpdateTrainingsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IBeeKeeperTrainingService _trainingsService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;
        private readonly IAHWDLookupService _ahwdLookupService;

        // Training year message color 
        private static readonly Color ErrorColor = Color.FromArgb("#D32F2F");
        private static readonly Color NormalColor = Color.FromArgb("#030303");

        //Training Fields
        [ObservableProperty] private int trainingCtr;
        [ObservableProperty] private int trainingYear;
        [ObservableProperty] private int numberOfHrs;
        [ObservableProperty] private bool errNumberOfHrsBool;
        [ObservableProperty] private bool errBeeTrainingsBool;
        [ObservableProperty] private Color errTrainingYearColor = Color.FromArgb("#030303");

        //Selected Items
        [ObservableProperty] private BeeTrainingResponseModel? selectedBeeTrainings;

        //Collection & List
        [ObservableProperty]
        private ObservableCollection<BeeTrainingResponseModel> beeTrainings = new();

        public List<int> Years { get; } =
            Enumerable.Range(1900, DateTime.Now.Year - 1900 + 1).ToList();

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("TrainingCtr", out var value))
                TrainingCtr = Convert.ToInt32(value);
        }

        public UpdateTrainingsViewModel(
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

        //loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            var allTask = _ahwdLookupService.GetAllBeeTrainingsAsync();
            var existingTask = _trainingsService.GetAllBeeKeeperTrainingsAsync();
            var currentTask = _trainingsService.GetTrainingByCtrAsync(TrainingCtr);

            await Task.WhenAll(allTask, existingTask, currentTask);

            var all = await allTask;
            var existing = await existingTask;
            var current = await currentTask;

            if (all is null || current is null) return;

            BeeTrainings.Clear();
            ErrBeeTrainingsBool = false;

            foreach (var item in all)
            {
                bool isDuplicate = existing?.Any(x =>
                    x.TrainingCode == item.TrainingCode &&
                    x.TrainingCode != current.TrainingCode) == true;

                if (isDuplicate) continue;
                BeeTrainings.Add(item!);
            }

            SelectedBeeTrainings = BeeTrainings
                .FirstOrDefault(x => x.TrainingCode == current.TrainingCode);
            NumberOfHrs = current.NumberofHrs;
            TrainingYear = current.TrainingYear;
        }

        //Selection Event
        [RelayCommand]
        public void SelectionBeeTrainings()
        {
            if (SelectedBeeTrainings == null) return;
            ErrBeeTrainingsBool = false;
        }

        //Update training command
        [RelayCommand]
        private async Task UpdateTrainingAsync()
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
                    var isUpdated = await _trainingsService.UpdateTrainingAsync(
                        new UpdateBeeKeeperTrainingsRequestModel
                        {
                            TrainingCtr = TrainingCtr,
                            TrainingCode = SelectedBeeTrainings!.TrainingCode,
                            TrainingDescription = SelectedBeeTrainings.TrainingDescription!,
                            TrainingYear = TrainingYear,
                            NumberofHrs = NumberOfHrs
                        });

                    if (!isUpdated) return;

                    SelectedBeeTrainings = new();
                    NumberOfHrs = TrainingYear = 0;
                    ErrNumberOfHrsBool = ErrBeeTrainingsBool = false;
                    ErrTrainingYearColor = NormalColor;

                    _popupService.ShowSuccessDialog("Training updated successfully");
                    await Shell.Current.GoToAsync("..");
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
