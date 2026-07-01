using BeeKeeperRegister.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services
{
    public class TempDataBeeSpecies
    {
        private int _counter = 1;
        private ObservableCollection<TempDataBeeSpeciesModel> TempData { get; } = new ObservableCollection<TempDataBeeSpeciesModel>();


        public ObservableCollection<TempDataBeeSpeciesModel> GetAllBeeSpecies()
        {
            return TempData;
        }

        public void AddBeeSpecies(TempDataBeeSpeciesModel item)
        {
            if (item == null) return;
            item.Id = _counter++;

            TempData.Add(item);
        }

        public void ClearAll()
        {
            TempData.Clear();
            _counter = 1;
        }

        public TempDataBeeSpeciesModel GetByID(int id)
        {
            return TempData
                .Where(x => x.Id == id)
                .Select(x => new TempDataBeeSpeciesModel
                {
                    Id = x.Id,
                    LocationId = x.LocationId,
                    BeeTypeId = x.BeeTypeId,
                    BeeTypeDescription = x.BeeTypeDescription,
                    NumberColonies = x.NumberColonies,
                    Bscolonies = x.Bscolonies,
                    BscoloniesDescription = x.BscoloniesDescription,
                    ProvCode = x.ProvCode,
                    ProvinceName = x.ProvinceName,
                    IfImported = x.IfImported,
                    Gmicntry = x.Gmicntry,
                    CountryName = x.CountryName
                })
                .FirstOrDefault();
        }

        public void UpdateBeeSpecies(TempDataBeeSpeciesModel updatedItem)
        {
            if (updatedItem == null) return;

            var existing = TempData.FirstOrDefault(x => x.Id == updatedItem.Id);

            if (existing != null)
            {
                existing.BeeTypeId = updatedItem.BeeTypeId;
                existing.BeeTypeDescription = updatedItem.BeeTypeDescription;
                existing.NumberColonies = updatedItem.NumberColonies;
                existing.Bscolonies = updatedItem.Bscolonies;
                existing.BscoloniesDescription = updatedItem.BscoloniesDescription;
                existing.ProvCode = updatedItem.ProvCode;
                existing.ProvinceName = updatedItem.ProvinceName;
                existing.IfImported = updatedItem.IfImported;
                existing.Gmicntry = updatedItem.Gmicntry;
                existing.CountryName = updatedItem.CountryName;
            }
        }

        public bool DeleteBeeSpecies(int id)
        {
            var item = TempData.FirstOrDefault(x => x.Id == id);

            if (item == null) return false;

            TempData.Remove(item);
            return true;
        }

    }
}
