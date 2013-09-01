using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Models;
using System;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class StockController : BaseController {

        public ActionResult Create(int id) {
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");

            var item = Db.Items.Find(id);
            if (item == null) return HttpNotFound("Cannot find item with specified ID");
            var stock = new Stock { ItemId = id, Item = item };

            return View(stock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stock stock) {
            var item = Db.Items.Find(stock.ItemId);
            if (item == null) return HttpNotFound("Cannot find item with specified itemId");

            if (ModelState.IsValid) {
                Db.Stocks.Add(stock);
                Db.SaveChanges();

                FlashSuccess(string.Format("{0} stock items have been added to {1}", stock.Quantity, item.Name));

                return RedirectToAction("Details", "Items", new { id = stock.ItemId });
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", stock.StockConditionId);

            stock = new Stock { ItemId = stock.ItemId, Item = item };

            return View(stock);
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
        public ActionResult Edit(Stock stock) {
            if (ModelState.IsValid) {
                Db.Entry(stock).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Details", "Items", new { id = stock.ItemId });
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", stock.StockConditionId);
            return View(stock);
        }

        public ActionResult Delete(int id = 0) {
            Stock item = Db.Stocks.Find(id);
            if (item == null) {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Stock item = Db.Stocks.Find(id);
            Db.Stocks.Remove(item);
            Db.SaveChanges();
            return RedirectToAction("Details", "Items", new { id = item.ItemId });
        }

        public ActionResult Move(int id) {
            var stock = Db.Stocks.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with specified ID");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(Stock stock) {
            if (ModelState.IsValid) {
                Db.Entry(stock).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Details", "Items", new { id = stock.ItemId });
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", stock.StockConditionId);
            return View(stock);
        }
    }
}