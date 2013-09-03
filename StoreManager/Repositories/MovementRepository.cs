using StoreManager.Models;

namespace StoreManager.Repositories {
    public class MovementRepository : EntityRepository<Movement> {
        public MovementRepository(StoreManagerContext db)
            : base(db) {
        }

        public override Movement Add(Movement entity) {
            var stockRepo = new StockRepository(Db);
            var locationRepo = new LocationRepository(Db);

            if (stockRepo.Find(entity.StockId) == null) {
                throw new EntityNotFoundException("Cannot find Stock with given ID");
            }

            if (locationRepo.Find(entity.LocationId) == null) {
                throw new EntityNotFoundException("Cannot find Location with given ID");
            }

            return base.Add(entity);
        }

        public void AttachAuthorizationDoc(int id, string path) {
            var movement = Find(id);
            if (movement == null) throw new EntityNotFoundException("Cannot find Movement with given ID");

            movement.AuthorizationDoc = path;

            Update(movement);
        }

        public void AttachRequisitionDoc(int id, string path) {
            var movement = Find(id);
            if (movement == null) throw new EntityNotFoundException("Cannot find Movement with given ID");

            movement.RequisitionDoc = path;

            Update(movement);
        }
    }
}