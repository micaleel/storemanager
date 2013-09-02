using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Models;
using StoreManager.Views.Stock;
using System;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class StockController : BaseController {

        public ActionResult Create(int id) {
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");

            var item = Db.Items.Find(id);
            if (item == null) return HttpNotFound("Cannot find item with specified ID");
            var stock = new CreateStockModel { ItemId = id, Item = item };

            return View(stock);
        }

        private static IEnumerable<Stock> Explode(CreateStockModel stock) {
            AutoMapper.Mapper.CreateMap<CreateStockModel, Stock>();

            stock.BatchId = Guid.NewGuid();

            for (var i = 0; i < stock.Quantity; i++) {
                var cloned = AutoMapper.Mapper.Map<CreateStockModel, Stock>(stock);

                cloned.IsParent = i == 0;

                yield return cloned;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateStockModel input) {
            var item = Db.Items.Find(input.ItemId);
            if (item == null) return HttpNotFound("Cannot find item with specified itemId");

            if (ModelState.IsValid) {

                foreach (var stockItem in Explode(input)) {
                    Db.Stocks.Add(stockItem);
                }

                Db.SaveChanges();

                FlashSuccess(string.Format("{0} stock items have been added to {1}", input.Quantity, item.Name));

                return RedirectToAction("Details", "Items", new { id = input.ItemId });
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", input.StockConditionId);

            input = new CreateStockModel { ItemId = input.ItemId, Item = item };

            return View(input);
        }

        public ActionResult Edit(int id = 0) {
            Stock item = Db.Stocks.Find(id);
            if (item == null) {
                return HttpNotFound();
            }
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", item.StockConditionId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stock stock, bool batchUpdate = false) {
            if (!ModelState.IsValid) {
                ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", stock.StockConditionId);
                return View(stock);
            }

            if (stock.IsParent && batchUpdate) {
                var batchItems = Db.Stocks.Where(x => x.BatchId == stock.BatchId && x.Id != stock.Id);

                foreach (var batchItem in batchItems) {
                    batchItem.Condition = stock.Condition;
                    batchItem.ExpiryDate = stock.ExpiryDate;
                    batchItem.PurchaseDate = stock.PurchaseDate;
                    batchItem.BatchPrice = stock.BatchPrice;
                    batchItem.UnitPrice = stock.UnitPrice;

                    Db.Entry(batchItem).State = EntityState.Modified;
                }
            }

            Db.Entry(stock).State = EntityState.Modified;
            Db.SaveChanges();

            return RedirectToAction("Details", "Items", new { id = stock.ItemId });
        }

        public ActionResult Delete(int id = 0) {
            Stock stock = Db.Stocks.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with given ID");
            return View(stock);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, bool deleteBatch = false) {
            Stock stock = Db.Stocks.Find(id);

            if (stock == null) return HttpNotFound("Cannot find Stock with given ID");

            var itemId = stock.ItemId;

            if (deleteBatch) {
                var batchStocks = Db.Stocks.Where(x => x.BatchId == stock.BatchId);
                foreach (var stockItem in batchStocks) {
                    Db.Stocks.Remove(stockItem);
                }
            }
            else {
                if (stock.IsParent) {
                    var nextParent = Db.Stocks.FirstOrDefault(x => x.BatchId == stock.BatchId);
                    if (nextParent != null) {
                        nextParent.IsParent = true;
                        Db.SaveChanges();
                    }
                }

                Db.Stocks.Remove(stock);

            }

            Db.SaveChanges();

            return RedirectToAction("Details", "Items", new { id = itemId });
        }

        public ActionResult Move(int id) {
            var stock = Db.Stocks.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with specified ID");
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");

            var viewModel = new MoveStockModel();
            viewModel.SetStock(stock);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(MoveStockModel input) {
            if (ModelState.IsValid) {

                var stock = Db.Stocks.Find(input.StockId);
                if (stock == null) return new HttpNotFoundResult("Cannot find Stock with given ID");

                // TODO:
                //stock.Quantity -= input.Quantity;

                var movement = new Movement {
                    DateCreated = DateTime.UtcNow,
                    StockId = input.StockId,
                    LocationId = input.LocationId,
                    Notes = input.Notes,
                    Quantity = input.Quantity
                };

                Db.Entry(stock).State = EntityState.Modified;
                Db.Movements.Add(movement);
                Db.SaveChanges();

                return RedirectToAction("Details", "Items", new { id = stock.ItemId });
            }

            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");
            return View(input);
        }
    }
}