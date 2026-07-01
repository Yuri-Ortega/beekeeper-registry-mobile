using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class UpdateTrainingsPage : ContentPage
{
    public UpdateTrainingsPage(UpdateTrainingsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UpdateTrainingsViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}