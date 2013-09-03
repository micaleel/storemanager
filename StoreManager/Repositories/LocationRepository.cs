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

            var movements = location.Movements.ToList();

            foreach (var movement in movements) {
                Db.Entry(movement).State = EntityState.Deleted;
            }

            Db.Locations.Remove(location);
            Db.SaveChanges();
        }
    }
}