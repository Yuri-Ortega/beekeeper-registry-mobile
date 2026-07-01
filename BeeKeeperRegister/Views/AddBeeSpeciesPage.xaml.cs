using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class AddBeeSpeciesPage : ContentPage
{
    public AddBeeSpeciesPage(AddBeeSpeciesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AddBeeSpeciesViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}