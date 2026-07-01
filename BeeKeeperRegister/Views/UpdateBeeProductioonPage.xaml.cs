using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class UpdateBeeProductioonPage : ContentPage
{
    public UpdateBeeProductioonPage(UpdateBeeProductioonViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UpdateBeeProductioonViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}