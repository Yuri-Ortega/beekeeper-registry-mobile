
using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Components.Views;
using BeeKeeperRegister.Handler;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using BeeKeeperRegister.ViewModels;
using BeeKeeperRegister.Views;
using DevExpress.Maui;
using DevExpress.Maui.Core;
using Mopups.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Net.Http.Headers;

namespace BeeKeeperRegister
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            ThemeManager.ApplyThemeToSystemBars = true;
            var builder = MauiApp.CreateBuilder()
                .UseMauiApp<App>()
                .UseDevExpress()
                .UseDevExpressCharts()
                .UseDevExpress(useLocalization: false)
                .UseDevExpressControls()
                .UseDevExpressCharts()
                .UseDevExpressTreeView()
                .UseDevExpressCollectionView()
                .UseDevExpressEditors()
                .UseDevExpressDataGrid()
                .UseDevExpressScheduler()
                .UseDevExpressGauges()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter-Medium.ttf", "InterMedium");
                    fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                    fonts.AddFont("roboto-medium.ttf", "Roboto-Medium");
                    fonts.AddFont("roboto-regular.ttf", "Roboto");
                }).ConfigureMopups()
                .UseMauiMaps();
            builder.Services.AddSingleton<LogoutHandler>();
            //builder.Services.AddSingleton<HttpStatusCodeHandler>();
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddTransient<JwtAuthHandler>();
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri(APIConfig.BaseURI);
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<JwtAuthHandler>();

            // Temp Data — Singleton
            builder.Services.AddSingleton<TempDataBeeSpecies>();
            builder.Services.AddSingleton<TempDataBeeProductioon>();

            // Dialog and Loading — Singleton
            builder.Services.AddSingleton<IDialogPopupService, DialogPopupService>();
            builder.Services.AddTransient<ILoadingPopupService, LoadingPopupService>();
            builder.Services.AddTransient<UsableLoadingDialogPopup>();
            builder.Services.AddTransient<UsableInvalidDialogPopup>();
            builder.Services.AddTransient<UsableSuccessDialogPopup>();
            builder.Services.AddTransient<UsableServerErrorDialogPopup>();
            builder.Services.AddTransient<UsableConfirmDialogPopup>();
            builder.Services.AddTransient<UsableGoogleMapDialogPopup>();
            builder.Services.AddTransient<GoogleMapPopupViewModel>();

            // Services — Transient
            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<IAHWDLookupService, AHWDLookupService>();
            builder.Services.AddTransient<IOneBAILookupService, OneBAILookupService>();
            builder.Services.AddTransient<IBeeKeeperRegistrationService, BeeKeeperRegistrationService>();
            builder.Services.AddTransient<IBeeKeeperTrainingService, BeeKeeperTrainingService>();
            builder.Services.AddTransient<IBeeKeeperFarmProfileService, BeeKeeperFarmProfileService>();
            builder.Services.AddTransient<IBeeLocationProductionTypeSourceService, BeeLocationProductionTypeSourceService>();
            builder.Services.AddTransient<IBeeLocationForageService, BeeLocationForageService>();
            builder.Services.AddTransient<IBeeProductioonService, BeeProductioonService>();
            builder.Services.AddTransient<IBeeProfileBioSecurityService, BeeProfileBioSecurityService>();
            builder.Services.AddTransient<IBeeFarmProfileAttachmentService, BeeFarmProfileAttachmentService>();

            // Pages and ViewModels — Transient
            builder.Services.AddTransient<LoadingPage>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<RegisterUserPage>();
            builder.Services.AddTransient<RegisterUserViewModel>();

            builder.Services.AddTransient<DashboardUserPage>();
            builder.Services.AddTransient<DashboardUserViewModel>();

            builder.Services.AddTransient<AddFarmProfilePage>();
            builder.Services.AddTransient<AddFarmProfileViewModel>();

            builder.Services.AddTransient<UpdateFarmProfilePage>();
            builder.Services.AddTransient<UpdateFarmProfileViewModel>();

            builder.Services.AddTransient<AddTrainingsPage>();
            builder.Services.AddTransient<AddTrainingsViewModel>();

            builder.Services.AddTransient<UpdateTrainingsPage>();
            builder.Services.AddTransient<UpdateTrainingsViewModel>();

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();

            builder.Services.AddTransient<AddTempBeeSpeciesPage>();
            builder.Services.AddTransient<AddTempBeeSpeciesViewModel>();

            builder.Services.AddTransient<UpdateTempBeeSpeciesPage>();
            builder.Services.AddTransient<UpdateTempBeeSpeciesViewModel>();

            builder.Services.AddTransient<AddTempBeeProductioonPage>();
            builder.Services.AddTransient<AddTempBeeProductioonViewModel>();

            builder.Services.AddTransient<UpdateTempBeeProductioonPage>();
            builder.Services.AddTransient<UpdateTempBeeProductioonViewModel>();

            builder.Services.AddTransient<AddBeeSpeciesPage>();
            builder.Services.AddTransient<AddBeeSpeciesViewModel>();

            builder.Services.AddTransient<UpdateBeeSpeciesPage>();
            builder.Services.AddTransient<UpdateBeeSpeciesViewModel>();

            builder.Services.AddTransient<AddBeeProductioonPage>();
            builder.Services.AddTransient<AddBeeProductioonViewModel>();

            builder.Services.AddTransient<UpdateBeeProductioonPage>();
            builder.Services.AddTransient<UpdateBeeProductioonViewModel>();

            builder.Services.AddTransient<ViewFarmProfilePage>();
            builder.Services.AddTransient<ViewFarmProfileViewModel>();

            builder.Services.AddTransient<EditProfileImagePage>();
            builder.Services.AddTransient<EditProfileImageViewModel>();

            return builder.Build();
        }

    }
}
