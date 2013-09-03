using StoreManager.Models;

namespace StoreManager.Repositories {
    public abstract class BaseRepository {
        protected readonly StoreManagerContext Db;

        protected BaseRepository(StoreManagerContext db) {
            Db = db;
        }
    }
}