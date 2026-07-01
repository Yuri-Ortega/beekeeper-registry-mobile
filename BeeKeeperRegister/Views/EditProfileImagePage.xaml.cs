using BeeKeeperRegister.ViewModels;

namespace BeeKeeperRegister.Views;

public partial class EditProfileImagePage : ContentPage
{
	public EditProfileImagePage(EditProfileImageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;

        if (BindingContext is EditProfileImageViewModel mvvm)
        {
            mvvm.ImageEditControl = editor;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is EditProfileImageViewModel vm)
        {
            await vm.LoaderAsync();
        }

    }
}