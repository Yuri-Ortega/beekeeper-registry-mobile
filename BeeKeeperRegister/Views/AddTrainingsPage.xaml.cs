using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class AddTrainingsPage : ContentPage
{
    public AddTrainingsPage(AddTrainingsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AddTrainingsViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}