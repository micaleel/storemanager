using DialogueStore.Web.Models;

namespace DialogueStore.Web.Repositories {
    public class StockConditionRepository : EntityRepository<StockCondition> {
        public StockConditionRepository(DialogueStoreContext db)
            : base(db) {
        }
    }
}