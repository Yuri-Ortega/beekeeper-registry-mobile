using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class AddTempBeeSpeciesPage : ContentPage
{
    public AddTempBeeSpeciesPage(AddTempBeeSpeciesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AddTempBeeSpeciesViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}