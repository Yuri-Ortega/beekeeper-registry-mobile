using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.UI
{
    public partial class BioSecurityUIModel : ObservableObject
    {
        [ObservableProperty]
        private bool? selectedBioSecurity;

        public string BeeBioCode { get; set; }
        public string BeeBioDescription { get; set; }

        public int ColumnNumNoBio { get; set; }


        public bool? IsviYesBio { get; set; }
        public bool? IsviNoBio { get; set; }

        public Color YesRadioBg =>
            SelectedBioSecurity == true
                ? (Color)Application.Current!.Resources["Success"]
                : Colors.Transparent;

        public Color NoRadioBg =>
            SelectedBioSecurity == false
                ? (Color)Application.Current!.Resources["Danger"]
                : Colors.Transparent;

        public Color YesRadioTxt =>
            SelectedBioSecurity == true
                ? (Color)Application.Current!.Resources["Surface"]
                : (Color)Application.Current!.Resources["Label"];

        public Color NoRadioTxt =>
            SelectedBioSecurity == false
                ? (Color)Application.Current!.Resources["Surface"]
                : (Color)Application.Current!.Resources["Label"];

        partial void OnSelectedBioSecurityChanged(bool? value)
        {
            OnPropertyChanged(nameof(YesRadioBg));
            OnPropertyChanged(nameof(NoRadioBg));
            OnPropertyChanged(nameof(YesRadioTxt));
            OnPropertyChanged(nameof(NoRadioTxt));
        }
    }
}
