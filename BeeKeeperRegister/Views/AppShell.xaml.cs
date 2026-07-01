
namespace BeeKeeperRegister.Views
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(RegisterUserPage), typeof(RegisterUserPage));
            Routing.RegisterRoute(nameof(DashboardUserPage), typeof(DashboardUserPage));
            Routing.RegisterRoute(nameof(AddFarmProfilePage), typeof(AddFarmProfilePage));
            Routing.RegisterRoute(nameof(UpdateFarmProfilePage), typeof(UpdateFarmProfilePage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(AddTrainingsPage), typeof(AddTrainingsPage));
            Routing.RegisterRoute(nameof(UpdateTrainingsPage), typeof(UpdateTrainingsPage));
            Routing.RegisterRoute(nameof(AddTempBeeSpeciesPage), typeof(AddTempBeeSpeciesPage));
            Routing.RegisterRoute(nameof(UpdateTempBeeSpeciesPage), typeof(UpdateTempBeeSpeciesPage));
            Routing.RegisterRoute(nameof(AddTempBeeProductioonPage), typeof(AddTempBeeProductioonPage));
            Routing.RegisterRoute(nameof(UpdateTempBeeProductioonPage), typeof(UpdateTempBeeProductioonPage));
            Routing.RegisterRoute(nameof(EditProfileImagePage), typeof(EditProfileImagePage));

            Routing.RegisterRoute(nameof(AddBeeSpeciesPage), typeof(AddBeeSpeciesPage));
            Routing.RegisterRoute(nameof(UpdateBeeSpeciesPage), typeof(UpdateBeeSpeciesPage));
            Routing.RegisterRoute(nameof(AddBeeProductioonPage), typeof(AddBeeProductioonPage));
            Routing.RegisterRoute(nameof(UpdateBeeProductioonPage), typeof(UpdateBeeProductioonPage));

            Routing.RegisterRoute(nameof(ViewFarmProfilePage), typeof(ViewFarmProfilePage));
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //SecureStorage.RemoveAll();
            var token = await SecureStorage.GetAsync("access_token");
            var refreshToken = await SecureStorage.GetAsync("refresh_token");
            if (string.IsNullOrEmpty(token) && string.IsNullOrEmpty(refreshToken))
                await Shell.Current.GoToAsync(nameof(LoginPage));
            else
            {
                await Shell.Current.GoToAsync(nameof(DashboardUserPage));
            }
        }
    }
}