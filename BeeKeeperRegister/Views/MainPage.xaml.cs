using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}