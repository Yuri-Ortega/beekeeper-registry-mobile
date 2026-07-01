using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class UpdateBeeSpeciesPage : ContentPage
{
    public UpdateBeeSpeciesPage(UpdateBeeSpeciesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UpdateBeeSpeciesViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}