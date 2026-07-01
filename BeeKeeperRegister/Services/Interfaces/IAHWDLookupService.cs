using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IAHWDLookupService
    {
        Task<List<BeeProductionSystemModel>?> GetAllBeeProductionSystemsAsync();
        Task<List<BeeCommonPestModel>?> GetAllBeeCommonPestsAsync();
        Task<List<BeeCommonDiseasesModel>?> GetAllBeeCommonDiseasesAsync();
        Task<List<BeeTrainingModel>?> GetAllBeeTrainingsAsync();
        Task<List<BeeForagesModel>?> GetAllBeeForagesAsync();
        Task<List<BeeTypesModel>?> GetAllBeeTypesAsync();
        Task<List<BeeSourceColoniesModel>?> GetAllBeeSourcesColoniesAsync();
        Task<List<BeeProductionModel>?> GetAllBeeProductionCategoriesAsync();
        Task<List<BeeBioSecurityModel>?> GetAllBeeBiosecuritiesAsync();
    }
}
