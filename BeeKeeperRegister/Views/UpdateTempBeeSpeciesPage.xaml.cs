using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class UpdateTempBeeSpeciesPage : ContentPage
{
    public UpdateTempBeeSpeciesPage(UpdateTempBeeSpeciesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UpdateTempBeeSpeciesViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}