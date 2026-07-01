using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class AddTempBeeProductioonPage : ContentPage
{
    public AddTempBeeProductioonPage(AddTempBeeProductioonViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AddTempBeeProductioonViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}