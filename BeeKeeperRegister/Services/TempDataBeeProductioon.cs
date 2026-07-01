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
    public class TempDataBeeProductioon
    {
        private int _counter = 1;
        private ObservableCollection<TempDataBeeProductioonModel> TempData { get; } = new ObservableCollection<TempDataBeeProductioonModel>();


        public ObservableCollection<TempDataBeeProductioonModel> GetAllBeeProductioon()
        {
            return TempData;
        }

        public void AddBeeProductioon(TempDataBeeProductioonModel item)
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

        public TempDataBeeProductioonModel GetByID(int id)
        {
            return TempData
                .Where(x => x.Id == id)
                .Select(x => new TempDataBeeProductioonModel
                {
                    Id = x.Id,
                    LocationId = x.LocationId,
                    BeeProdId = x.BeeProdId,
                    BeeProductionDescription = x.BeeProductionDescription,
                    EstProdYield = x.EstProdYield
                })
                .FirstOrDefault();
        }

        public void UpdateBeeProductioon(TempDataBeeProductioonModel updatedItem)
        {
            if (updatedItem == null) return;

            var existing = TempData.FirstOrDefault(x => x.Id == updatedItem.Id);

            if (existing != null)
            {
                existing.BeeProdId = updatedItem.BeeProdId;
                existing.BeeProductionDescription = updatedItem.BeeProductionDescription;
                existing.EstProdYield = updatedItem.EstProdYield;
            }
        }   

        public bool DeleteBeeProductioon(int id)
        {
            var item = TempData.FirstOrDefault(x => x.Id == id);

            if (item == null) return false;

            TempData.Remove(item);
            return true;
        }

    }
}
