using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeeKeeperRegister.Components.Classes
{
    public interface IDialogPopupService
    {
        void ShowInvalidDialog(string message);
        void ShowSuccessDialog(string message);
        void ShowServerErrorDialog(string message);

        Task<bool> ShowConfirmDialog(string title, string message, string leftButtonContent, string rightButtonContent);


        Task<GoogleMapResponseModel> ShowGoogleMapPopup(string region, string province, string municipality, string barangay, double? latitude = null, double? longitude = null);
    }
}
