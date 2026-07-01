using BeeKeeperRegister.Components.Views;
using Mopups.Interfaces;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Components.Classes
{
    public class LoadingPopupService : ILoadingPopupService, IDisposable
    {
        private readonly IPopupNavigation navigation;

        public LoadingPopupService()
        {
            navigation = MopupService.Instance;
        }

        public async void Dispose()
        {
            await navigation.PopAsync();
        }

        public async Task<IDisposable> Show()
        {
            await navigation.PushAsync(new UsableLoadingDialogPopup(), true);
            return this;
        }
    }
}
