using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.UI
{
    public class ViewFarmBioSecurityUIModel
    {
        public bool? SelectedBioSecurity { get; set; }
        public string BeeBioCode { get; set; } = string.Empty;
        public string BeeBioDescription { get; set; } = string.Empty;

        public bool IsYesBool => SelectedBioSecurity == true;
        public bool IsNoBool => SelectedBioSecurity == false;

        public Color YesRadioBg =>
            SelectedBioSecurity == true
                ? (Color)Application.Current!.Resources["Success"]
                : Colors.Transparent;

        public Color NoRadioBg =>
            SelectedBioSecurity == false
                ? (Color)Application.Current!.Resources["Warning"]
                : Colors.Transparent;

        public Color YesRadioTxt =>
            SelectedBioSecurity == true
                ? (Color)Application.Current!.Resources["Surface"]
                : (Color)Application.Current!.Resources["Success"];

        public Color NoRadioTxt =>
            SelectedBioSecurity == false
                ? (Color)Application.Current!.Resources["Surface"]
                : (Color)Application.Current!.Resources["Warning"];
    }
}
