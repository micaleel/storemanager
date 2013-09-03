using System.Collections.Generic;
using System.Data;
using StoreManager.Models;
using System.Linq;

namespace StoreManager.Repositories {
    public class LocationRepository : EntityRepository<Location> {

        public LocationRepository(StoreManagerContext context)
            : base(context) {
        }

        public override void Delete(int id) {
            var location = Find(id);
            if (location == null) throw new System.ApplicationException("Cannot find entity with given ID");
            if (location.IsStore) throw new System.ApplicationException("Cannot delete a store location");

            var allMovements = location.FromMovements.Union(location.ToMovements).ToList();

            foreach (var movement in allMovements) {
                Db.Entry(movement).State = EntityState.Deleted;
            }

            Db.Locations.Remove(location);
            Db.SaveChanges();
        }

        public Location GetOrCreateStoreLocation() {
            var storeLocation = Db.Locations.FirstOrDefault(x => x.IsStore);
            if (storeLocation != null) return storeLocation;

            storeLocation = new Location {
                IsStore = true,
                Name = "Store (auto-created)",
                Notes = "Created by StoreManager"
            };

            Db.Locations.Add(storeLocation);
            Db.SaveChanges();

            return storeLocation;
        }
    }
}