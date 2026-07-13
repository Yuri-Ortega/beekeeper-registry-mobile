
using System.Diagnostics;

namespace BeeKeeperRegister.Views
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
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

            Loaded += OnShellLoaded;
        }


        private async void OnShellLoaded(object? sender, EventArgs e) { await CheckAuthenticationAsync(); }

        private async Task CheckAuthenticationAsync()
        {
            try
            {
                //SecureStorage.RemoveAll();
                var token = await SecureStorage.GetAsync("access_token");
                var isAuthenticated = !string.IsNullOrEmpty(token);

                if (isAuthenticated)
                {
                    await GoToAsync("//DashboardPage");
                }
                else
                {
                    await GoToAsync("//LoginPage");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Auth check error: {ex.Message}");
                await GoToAsync("//LoginPage");
            }
        }
    }
}