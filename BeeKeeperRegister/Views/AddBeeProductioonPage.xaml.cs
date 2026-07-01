using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class AddBeeProductioonPage : ContentPage
{
    public AddBeeProductioonPage(AddBeeProductioonViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AddBeeProductioonViewModel vm)
        {
            await vm.LoaderAsync();

        }
        
    }
}