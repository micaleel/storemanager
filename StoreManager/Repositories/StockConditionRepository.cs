using StoreManager.Models;

namespace StoreManager.Repositories {
    public class StockConditionRepository : EntityRepository<StockCondition> {
        public StockConditionRepository(StoreManagerContext db)
            : base(db) {
        }
    }
}