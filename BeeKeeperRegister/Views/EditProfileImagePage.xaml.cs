using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class EditProfileImagePage : ContentPage
{
	public EditProfileImagePage(EditProfileImageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is EditProfileImageViewModel vm)
        {
            vm.ImageEditControl = editor;
            await vm.LoaderAsync();
        }

    }
}