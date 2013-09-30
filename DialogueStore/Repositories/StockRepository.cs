using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DialogueStore.Models;

namespace DialogueStore.Repositories {
    public class StockRepository : EntityRepository<Stock> {
        public StockRepository(DialogueStoreContext db)
            : base(db) {
        }

        public IEnumerable<Stock> AddMany(IEnumerable<Stock> stocks) {
            var stockList = stocks as IList<Stock> ?? stocks.ToList();

            foreach (var stock in stockList) {
                Db.Stocks.Add(stock);
            }

            Db.SaveChanges();

            return stockList;
        }

        public Stock Update(Stock stock, bool updateBatch) {
            if (stock.IsParent && updateBatch) {
                var batchItems = All.Where(x => x.BatchId == stock.BatchId && x.Id != stock.Id);

                foreach (var batchItem in batchItems) {
                    batchItem.Condition = stock.Condition;
                    batchItem.ExpiryDate = stock.ExpiryDate;
                    batchItem.PurchaseDate = stock.PurchaseDate;
                    batchItem.BatchPrice = stock.BatchPrice;
                    batchItem.UnitPrice = stock.UnitPrice;

                    Db.Entry(batchItem).State = EntityState.Modified;
                }
            }

            Update(stock);

            return stock;
        }

        public override void Delete(int id) {
            var stock = Find(id);
            if (stock == null) throw new EntityNotFoundException("Cannot find stock with given ID");

            if (stock.IsParent) {
                var nextParent = All.FirstOrDefault(x => x.BatchId == stock.BatchId);
                if (nextParent != null) {
                    nextParent.IsParent = true;
                    Db.SaveChanges();
                }
            }

            base.Delete(id);
        }

        public void DeleteByBatchId(Guid batchId) {
            var affectedEntities = All.Where(x => x.BatchId == batchId);

            foreach (var entity in affectedEntities) {
                Db.Stocks.Remove(entity);
            }

            Db.SaveChanges();
        }
    }
}