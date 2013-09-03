using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using StoreManager.Models;

namespace StoreManager.Repositories {
    public class ItemRepository : EntityRepository<Item> {
        public ItemRepository(StoreManagerContext db)
            : base(db) {
        }

        public override void Delete(int id) {
            var item = Find(id);
            if (item == null) throw new ApplicationException("Cannot find entity with given ID");

            var stocks = item.Stocks.ToList();

            foreach (var stock in stocks) {
                Db.Entry(stock).State = EntityState.Deleted;
            }

            var movements = item.Movements.ToList();

            foreach (var movement in movements) {
                Db.Entry(movement).State = EntityState.Deleted;
            }

            Db.Items.Remove(item);
            Db.SaveChanges();
        }

        public Stock AddStock(int itemId, Stock stock) {
            var item = Find(itemId);
            if (item == null) throw new EntityNotFoundException("Cannot find Item with given ID");

            var locationRepo = new LocationRepository(Db);
            var storeLocation = locationRepo.GetOrCreateStoreLocation();

            stock.LocationId = storeLocation.Id;

            var stockRepo = new StockRepository(Db);

            return stockRepo.Add(stock);
        }

        public IEnumerable<Stock> AddStocks(int itemId, IEnumerable<Stock> stocks) {
            var item = Find(itemId);
            if (item == null) throw new EntityNotFoundException("Cannot find Item with given ID");

            var locationRepo = new LocationRepository(Db);
            var storeLocation = locationRepo.GetOrCreateStoreLocation();
            var stockList = stocks as IList<Stock> ?? stocks.ToList();

            if (!stockList.Any(x => x.IsParent)) {
                stockList.First().IsParent = true;
            }

            var stockRepo = new StockRepository(Db);
            var addedStocks = stockRepo.AddMany(stockList).ToList();

            foreach (var stock in addedStocks) {
                MoveStock(new Movement { StockId = stock.Id, LocationId = storeLocation.Id });
            }

            return addedStocks;
        }

        [Obsolete]
        public Movement MoveStock(int stockId, int locationId, string notes = "") {
            var locationRepo = new LocationRepository(Db);
            var movementRepo = new MovementRepository(Db);
            var stockRepo = new StockRepository(Db);

            var stock = stockRepo.Find(stockId);
            if (stock == null) throw new EntityNotFoundException("Cannot find Stock with given ID");

            var location = locationRepo.Find(locationId);
            if (location == null) throw new EntityNotFoundException("Cannot find Location with given ID");

            stock.LocationId = location.Id;
            stockRepo.Update(stock);

            var movement = new Movement {
                DateCreated = DateTime.UtcNow,
                StockId = stock.Id,
                LocationId = location.Id,
                Notes = notes
            };

            movementRepo.Add(movement);
            Db.SaveChanges();

            return movement;
        }

        public Movement MoveStock(Movement movement) {
            var locationRepo = new LocationRepository(Db);
            var movementRepo = new MovementRepository(Db);
            var stockRepo = new StockRepository(Db);

            var stock = stockRepo.Find(movement.StockId);
            if (stock == null) throw new EntityNotFoundException("Cannot find Stock with given ID");

            var location = locationRepo.Find(movement.LocationId);
            if (location == null) throw new EntityNotFoundException("Cannot find Location with given ID");

            stock.LocationId = location.Id;
            stockRepo.Update(stock);

            movement.DateCreated = DateTime.UtcNow;

            movementRepo.Add(movement);
            Db.SaveChanges();

            return movement;
        }

    }
}