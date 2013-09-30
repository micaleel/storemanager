using DialogueStore.Models;

namespace DialogueStore.Repositories {
    public class StockConditionRepository : EntityRepository<StockCondition> {
        public StockConditionRepository(DialogueStoreContext db)
            : base(db) {
        }
    }
}