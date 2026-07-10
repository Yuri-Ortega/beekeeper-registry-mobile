using BeeKeeperRegister.Components.Views;
using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeeKeeperRegister.Components.Classes
{
    public class DialogPopupService : IDialogPopupService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogPopupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> ShowConfirmDialog(string title, string message, string leftButtonContent, string rightButtonContent)
        {
            var popup = _serviceProvider.GetRequiredService<UsableConfirmDialogPopup>();

            var viewModel = new DialogPopupViewModel(title, message, leftButtonContent, rightButtonContent);
            popup.BindingContext = viewModel;
            viewModel.IsOpen = true;
            popup.Show();

            var result = await viewModel.WaitForResultAsync();

            popup.Close();

            return result;
        }

        public void ShowInvalidDialog(string message)
        {
            var popup = _serviceProvider.GetRequiredService<UsableInvalidDialogPopup>();

            var viewModel = new DialogPopupViewModel(message, popup);
            popup.BindingContext = viewModel;
            popup.Show();
        }

        public void ShowServerErrorDialog(string message)
        {
            var popup = _serviceProvider.GetRequiredService<UsableServerErrorDialogPopup>();

            var viewModel = new DialogPopupViewModel(message, popup);
            popup.BindingContext = viewModel;
            popup.Show();
        }

        public void ShowSuccessDialog(string message)
        {
            var popup = _serviceProvider.GetRequiredService<UsableSuccessDialogPopup>();

            var viewModel = new DialogPopupViewModel(message, popup);
            popup.BindingContext = viewModel;
            popup.Show();
        }

        public async Task<GoogleMapResponseModel> ShowGoogleMapPopup(
    string region,
    string province,
    string municipality,
    string barangay,
    double? latitude = null,
    double? longitude = null)
        {
            var popup = _serviceProvider.GetRequiredService<UsableGoogleMapDialogPopup>();

            var viewModel = new GoogleMapPopupViewModel(region, province, municipality, barangay, latitude, longitude);

            popup.BindingContext = viewModel;
            popup.Show();

            var result = await viewModel.WaitForResultAsync();

            popup.Close();

            return result;
        }
    }
}
