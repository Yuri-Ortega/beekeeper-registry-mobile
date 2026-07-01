using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Components.Classes
{
    public interface ILoadingPopupService
    {
        Task<IDisposable> Show();
    }
}
