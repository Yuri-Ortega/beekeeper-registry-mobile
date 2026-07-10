using BeeKeeperRegister.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IAHWDLookupService
    {
        Task<List<BeeProductionSystemResponseModel>?> GetAllBeeProductionSystemsAsync();
        Task<List<BeeCommonPestResponseModel>?> GetAllBeeCommonPestsAsync();
        Task<List<BeeCommonDiseasesResponseModel>?> GetAllBeeCommonDiseasesAsync();
        Task<List<BeeTrainingResponseModel>?> GetAllBeeTrainingsAsync();
        Task<List<BeeForagesResponseModel>?> GetAllBeeForagesAsync();
        Task<List<BeeTypesResponseModel>?> GetAllBeeTypesAsync();
        Task<List<BeeSourceColoniesResponseModel>?> GetAllBeeSourcesColoniesAsync();
        Task<List<BeeProductionResponseModel>?> GetAllBeeProductionCategoriesAsync();
        Task<List<BeeBioSecurityResponseModel>?> GetAllBeeBiosecuritiesAsync();
    }
}
