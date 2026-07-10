using BeeKeeperRegister.Components.Classes;
using BeeKeeperRegister.Models.Request;
using BeeKeeperRegister.Models.Response;
using BeeKeeperRegister.Services;
using BeeKeeperRegister.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace BeeKeeperRegister.ViewModels
{
    public partial class RegisterUserViewModel : ObservableObject
    {
        private readonly IOneBAILookupService _oneBAILookupService;
        private readonly IBeeKeeperRegistrationService _beeKeeperRegisterService;
        private readonly IAccountService _accountService;
        private readonly ILoadingPopupService _loading;
        private readonly IDialogPopupService _popupService;

        // Account Fields
        [ObservableProperty] private string? user;
        [ObservableProperty] private string? contactNumber;
        [ObservableProperty] private string? email;
        [ObservableProperty] private string? pass;
        [ObservableProperty] private string? confPass;

        // Error Messages Account
        [ObservableProperty]
        private string? errPassTxt;

        [ObservableProperty]
        private string? errConfPassTxt;

        [ObservableProperty]
        private string? errEmailTxt;

        [ObservableProperty]
        private string? errContactTxt;

        // Error Flags Account
        [ObservableProperty] private bool errUserBool;
        [ObservableProperty] private bool errContactBool;
        [ObservableProperty] private bool errEmailBool;
        [ObservableProperty] private bool errPassBool;
        [ObservableProperty] private bool errConfPassBool;

        // Personal Info Fields
        [ObservableProperty] private string? fN;
        [ObservableProperty] private string? lN;
        [ObservableProperty] private string? mN;
        [ObservableProperty] private string? eXN;
        [ObservableProperty] private DateTime birthday = DateTime.MinValue;
        [ObservableProperty] private DateTime startBeeKeeping = DateTime.MinValue;

        // Error Flags Personal Info
        [ObservableProperty] private bool errFNBool;
        [ObservableProperty] private bool errLNBool;
        [ObservableProperty] private bool errBirthdayBool;
        [ObservableProperty] private bool errSexBool;
        [ObservableProperty] private bool errRegionBool;
        [ObservableProperty] private bool errProvinceBool;
        [ObservableProperty] private bool errMunicipalityBool;
        [ObservableProperty] private bool errBarangayBool;
        [ObservableProperty] private Color? errLocationColor;

        // BeeKeeper Info Fields
        [ObservableProperty] private string selectedPMB = "No";
        [ObservableProperty] private string selectedIsHobbyist = "No";
        [ObservableProperty] private string ifYesPMB = string.Empty;
        [ObservableProperty] private int noFP;
        [ObservableProperty] private bool errStartBeeKeepingBool;
        [ObservableProperty] private bool errNoFPBool;
        [ObservableProperty] private bool isYesPMBBool;

        // Location Fields
        [ObservableProperty] private double latitude;
        [ObservableProperty] private double longitude;
        [ObservableProperty] private bool isEnabledSelectLocation;
            
        // Location Dropdowns
        [ObservableProperty] private bool isEnabledProvince;
        [ObservableProperty] private bool isEnabledMunicipality;
        [ObservableProperty] private bool isEnabledBarangay;

        // Collections  
        [ObservableProperty]
        private ObservableCollection<SexTypesResponseModel> sex = new();

        [ObservableProperty]
        private ObservableCollection<RegionResponseModel> region = new();

        [ObservableProperty]
        private ObservableCollection<ProvinceResponseModel> province = new();

        [ObservableProperty]
        private ObservableCollection<MunicipalityResponseModel> municipality = new();

        [ObservableProperty]
        private ObservableCollection<BarangayResponseModel> barangay = new();


        // Selected Items
        [ObservableProperty] private SexTypesResponseModel? selectedSex;
        [ObservableProperty] private RegionResponseModel? selectedRegion;
        [ObservableProperty] private ProvinceResponseModel? selectedProvince;
        [ObservableProperty] private MunicipalityResponseModel? selectedMunicipality;
        [ObservableProperty] private BarangayResponseModel? selectedBarangay;


        // Step
        [ObservableProperty] private int currentStep;


        public RegisterUserViewModel(
            IOneBAILookupService oneBAILookupService,
            ILoadingPopupService loading,
            IBeeKeeperRegistrationService beeKeeperRegisterService,
            IAccountService accountService,
            IDialogPopupService popupService)
        {
            _oneBAILookupService = oneBAILookupService;
            _beeKeeperRegisterService = beeKeeperRegisterService;
            _accountService = accountService;
            _loading = loading;
            _popupService = popupService;
        }


        // Loader
        [RelayCommand]
        public async Task LoaderAsync()
        {
            await Task.WhenAll(
                LoadSexTypesAsync(),
                LoadRegionsAsync()
            );
        }

        private async Task LoadSexTypesAsync()
        {
            var sexTypes = await _oneBAILookupService.GetSexTypesAsync();
            if (sexTypes is null) return;
            Sex.Clear();
            foreach (var item in sexTypes) Sex.Add(item);
        }

        private async Task LoadRegionsAsync()
        {
            var regions = await _oneBAILookupService.GetRegionsAsync();
            if (regions is null) return;
            Region.Clear();
            foreach (var item in regions) Region.Add(item!);
        }


        // Register Account Command
        [RelayCommand]
        private async Task RegisterAccountAsync()
        {
            try
            {
                using (await _loading.Show())
                {
                    var isCreatedAccount = await _accountService.RegisterUserAsync(BuildRegisterUser());


                    var isRegisteredBeeKeeper = await _beeKeeperRegisterService
                        .RegisterBeeKeeperProfileAsync(BuildBeeKeeperModel());

                    if (isCreatedAccount == true && isRegisteredBeeKeeper)
                        ResetForm();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private AddBeeKeeperRegisterRequestModel BuildBeeKeeperModel() => new()
        {
            Firstname = FN!,
            Lastname = LN!,
            ClientSexId = SelectedSex!.SexId,
            ClientSexDescription = SelectedSex.SexDescription!,
            ClientRcode = SelectedRegion!.Rcode,
            ClientRegion = SelectedRegion.Region!,
            ClientProvCode = SelectedProvince!.ProvCode,
            ClientProvinceName = SelectedProvince.ProvinceName!,
            ClientMunCode = SelectedMunicipality!.MunCode,
            ClientMunicipalities = SelectedMunicipality.MunCity!,
            ClientBcode = SelectedBarangay!.Bcode,
            ClientBarangayName = SelectedBarangay.BarangayName!,
            Birthday = DateOnly.FromDateTime(Birthday),
            Longitude = Longitude.ToString(),
            Latitude = Latitude.ToString(),
            StartBeekeepingMonthYear = StartBeeKeeping,
            PracMigraBeekeeping = SelectedPMB == "Yes",
            IfYesMigrateRemarks = IfYesPMB,
            NoFarmPersonnel = NoFP,
            IsHobbyist = SelectedIsHobbyist == "Yes",
            UserName = User!
        };

        private RegisterUserRequestModel BuildRegisterUser() => new()
        {
            UserName = User!,
            Email = Email!,
            PhoneNumber = ContactNumber!,
            Password = Pass!,
            Firstname = FN!,
            Lastname = LN!,
            MiddleName = MN! ?? "",
            ExtensionName = EXN! ?? "",
            SexId = SelectedSex!.SexId,
            SexDescription = SelectedSex.SexDescription!,
            Birthday = DateOnly.FromDateTime(Birthday)
        };


        // Validation
        public bool ValidateStep()
        {
            return CurrentStep switch
            {
                0 => ValidateStepZero(),
                1 => ValidateStepOne(),
                2 => ValidateStepTwo(),
                3 => ValidateStepThree(),
                _ => true
            };
        }

        private bool ValidateStepZero()
        {
            ErrUserBool = string.IsNullOrWhiteSpace(User);
            ErrContactBool = string.IsNullOrWhiteSpace(ContactNumber);
            ErrEmailBool = string.IsNullOrWhiteSpace(Email);
            ErrPassBool = string.IsNullOrWhiteSpace(Pass);
            ErrConfPassBool = string.IsNullOrWhiteSpace(ConfPass);
            return !(ErrUserBool || ErrContactBool || ErrEmailBool
                     || ErrPassBool || ErrConfPassBool);
        }

        private bool ValidateStepOne()
        {
            ErrFNBool = string.IsNullOrWhiteSpace(FN);
            ErrLNBool = string.IsNullOrWhiteSpace(LN);
            ErrSexBool = SelectedSex == null;
            ErrBirthdayBool = Birthday == DateTime.MinValue;
            return !(ErrFNBool || ErrLNBool || ErrSexBool || ErrBirthdayBool);
        }

        private bool ValidateStepTwo()
        {
            var LongLatRequire = (Longitude == 0 || Latitude == 0);
            ErrLocationColor = LongLatRequire ? Color.FromArgb("#B5402E") : Color.FromArgb("#C9962F");

        ErrRegionBool = SelectedRegion == null;
            ErrProvinceBool = SelectedProvince == null;
            ErrMunicipalityBool = SelectedMunicipality == null;
            ErrBarangayBool = SelectedBarangay == null;
            return !(ErrRegionBool || ErrProvinceBool
                     || ErrMunicipalityBool || ErrBarangayBool || LongLatRequire);
        }

        private bool ValidateStepThree()
        {
            ErrStartBeeKeepingBool = StartBeeKeeping == DateTime.MinValue;
            ErrNoFPBool = NoFP == 0;
            return !(ErrStartBeeKeepingBool || ErrNoFPBool);
        }


        // Selection Events
        [RelayCommand]
        public void BirthdayChanged()
        {
            ErrBirthdayBool = Birthday == DateTime.MinValue;
        }

        [RelayCommand]
        public void StartBeeKeepingChanged()
        {
            ErrStartBeeKeepingBool = StartBeeKeeping == DateTime.MinValue;
        }

        [RelayCommand]
        public void SelectionSex()
        {
            if (SelectedSex == null) return;
            ErrSexBool = false;
        }

        [RelayCommand]
        public async Task SelectionRegionAsync()
        {
            if (SelectedRegion == default) return;
            IsEnabledProvince = true;
            ErrRegionBool = false;
            Province.Clear();

            var provinces = await _oneBAILookupService.GetProvincesByRcodeAsync(SelectedRegion.Rcode);
            if (provinces is null) return;

            foreach (var item in provinces) Province.Add(item);
            SelectedProvince = null;
            SelectedMunicipality = null;
            SelectedBarangay = null;
        }

        [RelayCommand]
        public async Task SelectionProvinceAsync()
        {
            if (SelectedProvince == null) return;
            IsEnabledMunicipality = true;
            ErrProvinceBool = false;
            Municipality.Clear();

            var municipalities = await _oneBAILookupService.GetMunicipalitiesByProvCodeAsync(SelectedProvince.ProvCode);
            if (municipalities is null) return;

            foreach (var item in municipalities) Municipality.Add(item);
            SelectedMunicipality = null;
            SelectedBarangay = null;
        }

        [RelayCommand]
        public async Task SelectionMunicipalityAsync()
        {
            if (SelectedMunicipality == null) return;
            IsEnabledBarangay = true;
            ErrMunicipalityBool = false;
            Barangay.Clear();

            var barangays = await _oneBAILookupService.GetBarangaysByMunCodeAsync(SelectedMunicipality.MunCode);
            if (barangays is null) return;

            foreach (var item in barangays) Barangay.Add(item);
            SelectedBarangay = null;
        }

        [RelayCommand]
        public void SelectionBarangay()
        {
            if (SelectedBarangay == null)
            {
                IsEnabledSelectLocation = false;
                return;
            }
            ErrBarangayBool = false;
            Latitude = 0.0;
            Longitude = 0.0;
            IsEnabledSelectLocation = true;
        }


        // Map
        [RelayCommand]
        private async Task MapLocationBtnAsync()
        {
            if (SelectedBarangay == null || SelectedMunicipality == null ||
                SelectedProvince == null || SelectedRegion == null) return;

            var result = await _popupService.ShowGoogleMapPopup(
                SelectedRegion.Region!,
                SelectedProvince.ProvinceName!,
                SelectedMunicipality.MunCity!,
                SelectedBarangay.BarangayName!);

            if (result != null)
            {
                Latitude = result.Latitude;
                Longitude = result.Longitude;
                ErrLocationColor = Color.FromArgb("#C9962F");

            }
        }


        // Navigation Back to Login Command
        [RelayCommand]
        private async Task BackToLogin() =>
            await Shell.Current.GoToAsync("..");


        // Property Changed Handlers
        partial void OnSelectedPMBChanged(string value)
        {
            IsYesPMBBool = value == "Yes";
        }

        partial void OnUserChanged(string? value)
        {
            ErrUserBool = string.IsNullOrEmpty(value);
        }

        partial void OnContactNumberChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ErrContactBool = true;
            }
            else if (value.Length != 11)
            {
                ErrContactBool = true;
                ErrContactTxt = "*Contact number must have 11 digits";
            }
            else
            {
                ErrContactBool = false;
                ErrContactTxt = string.Empty;
            }
        }

        partial void OnEmailChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ErrEmailBool = true;
            }
            else if (!value.Contains('@'))
            {
                ErrEmailBool = true;
                ErrEmailTxt = "*Invalid E-mail address (missing '@')";
            }
            else if (!value.Contains('.'))
            {
                ErrEmailBool = true;
                ErrEmailTxt = "*Invalid E-mail address (missing '.')";
            }
            else
            {
                ErrEmailBool = false;
                ErrEmailTxt = string.Empty;
            }
        }

        partial void OnPassChanged(string? value)
        {

            if (string.IsNullOrEmpty(value))
            {
                ErrPassBool = true;
            }
            else if (!value.Any(char.IsUpper))
            {
                ErrPassBool = true;
                ErrPassTxt = "*Uppercase is Required";
            }
            else if (value.Length < 8)
            {
                ErrPassBool = true;
                ErrPassTxt = "*Password at least 8 characters";
            }
            else
            {
                ErrPassBool = false;
                ErrPassTxt = string.Empty;
            }
        }

        partial void OnConfPassChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ErrConfPassBool = true;
            }
            else if (!value.Any(char.IsUpper))
            {
                ErrConfPassBool = true;
                ErrConfPassTxt = "*Uppercase is Required";
            }
            else if (value.Length < 8)
            {
                ErrConfPassBool = true;
                ErrConfPassTxt = "*Confirm password at least 8 characters";
            }
            else if (value != Pass)
            {
                ErrConfPassBool = true;
                ErrConfPassTxt = "*Confirm Passwords do not match";
            }
            else
            {
                ErrConfPassBool = false;
                ErrConfPassTxt = string.Empty;
            }
        }

        partial void OnFNChanged(string? value)
        {
            ErrFNBool = string.IsNullOrEmpty(value);
        }

        partial void OnLNChanged(string? value)
        {
            ErrLNBool = string.IsNullOrEmpty(value);
        }

        partial void OnNoFPChanged(int value)
        {
            ErrNoFPBool = value == 0;
        }


        // Reset Form
        public void ResetForm()
        {
            User = Email = Pass = ConfPass =
            FN = LN = MN = EXN =
            ContactNumber = string.Empty;

            SelectedSex = new();
            SelectedRegion = new();
            SelectedProvince = new();
            SelectedMunicipality = new();
            SelectedBarangay = new();

            NoFP = 0;
            SelectedPMB = "No";
            SelectedIsHobbyist = "No";
            IfYesPMB = string.Empty;

            Birthday = DateTime.MinValue;
            StartBeeKeeping = DateTime.MinValue;

            Latitude = 0.0;
            Longitude = 0.0;
            IsEnabledSelectLocation = false;
            IsEnabledProvince = false;
            IsEnabledMunicipality = false;
            IsEnabledBarangay = false;

            ErrUserBool = ErrContactBool = ErrEmailBool =
            ErrPassBool = ErrConfPassBool = false;
            ErrFNBool = ErrLNBool = ErrSexBool =
            ErrBirthdayBool = false;
            ErrRegionBool = ErrProvinceBool =
            ErrMunicipalityBool = ErrBarangayBool = false;
            ErrStartBeeKeepingBool = ErrNoFPBool = false;

            CurrentStep = 0;
            _popupService.ShowSuccessDialog("Account created successfully");
        }
    }
}
