using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Components.Classes
{
    public partial class DialogPopupViewModel : ObservableObject
    {
        private readonly DXPopup? _popup;

        [ObservableProperty]
        private string dialogMessage;

        [ObservableProperty]
        private string? title;

        public DialogPopupViewModel(string message, DXPopup popup)
        {
            _popup = popup;
            DialogMessage = message;
        }

        private TaskCompletionSource<bool>? _tcs;

        [ObservableProperty]
        private string? leftBTNContent;

        [ObservableProperty]
        private string? rightBTNContent;

        [ObservableProperty]
        private bool isOpen;

        public DialogPopupViewModel(string title, string message, string leftButtonContent, string rightButtonContent)
        {
            Title = title;
            DialogMessage = message;
            LeftBTNContent = leftButtonContent;
            RightBTNContent = rightButtonContent;
            _tcs = new TaskCompletionSource<bool>();
        }

        public Task<bool> WaitForResultAsync()
        {
            return _tcs!.Task;
        }

        [RelayCommand]
        private void LeftBTN()
        {
            IsOpen = false;
            _tcs!.TrySetResult(true);
        }

        [RelayCommand]
        private void RightBTN()
        {
            IsOpen = false;
            _tcs!.TrySetResult(false);
        }
    }
}
