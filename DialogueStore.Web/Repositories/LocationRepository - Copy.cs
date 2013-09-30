using System.Data;
using Elmah;
using StoreManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreManager.Repositories {
    public class LocationRepository : BaseRepository {
        public LocationRepository(StoreManagerContext context)
            : base(context) {
        }

        public IEnumerable<Location> All {
            get { return Db.Locations; }
        }

        public Location Find(params object[] keyValues) {
            return Db.Locations.Find(keyValues);
        }

        public Location Add(Location entity) {
            Db.Locations.Add(entity);
            Db.SaveChanges();

            return entity;
        }

        public void Delete(int id) {
            var location = Find(id);
            if (location == null) throw new System.ApplicationException("Cannot find entity with given ID");
            if (location.IsStore) throw new System.ApplicationException("Cannot delete a store location");

            var movements = location.Movements.ToList();

            foreach (var movement in movements) {
                Db.Entry(movement).State = EntityState.Deleted;
            }

            Db.Locations.Remove(location);
            Db.SaveChanges();
        }
    }
}