using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class UpdateTempBeeProductioonPage : ContentPage
{
    public UpdateTempBeeProductioonPage(UpdateTempBeeProductioonViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UpdateTempBeeProductioonViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}