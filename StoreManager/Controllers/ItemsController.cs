using System.Data;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Models;
using System;

namespace StoreManager.Controllers {
    [AuthorizeAndRedirect]
    public class ItemsController : BaseController {

        public ActionResult Index() {
            var items = Db.Items;
            return View(items.ToList());
        }

        public ActionResult Details(int id = 0) {
            Item item = Db.Items.Find(id);
            if (item == null) {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult Create() {
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item, int locationId) {
            if (ModelState.IsValid) {
                Db.Items.Add(item);
                Db.SaveChanges();

                var locationName = Db.Locations.Single(l => l.Id == locationId).Name;
                Db.Movements.Add(new Movement { Date = DateTime.UtcNow, ItemId = item.Id, LocationId = locationId, Notes = "Item created and placed at " + locationName });
                Db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", item.StockConditionId);
            return View(item);
        }

        public ActionResult Edit(int id = 0) {
            Item item = Db.Items.Find(id);
            if (item == null) {
                return HttpNotFound();
            }
            //ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", item.StockConditionId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item) {
            if (ModelState.IsValid) {
                Db.Entry(item).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", item.StockConditionId);
            return View(item);
        }

        public ActionResult Delete(int id = 0) {
            Item item = Db.Items.Find(id);
            if (item == null) {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Item item = Db.Items.Find(id);
            Db.Items.Remove(item);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Movements(int id) {
            var movements = Db.Movements.Where(m => m.ItemId == id).OrderByDescending(m => m.Date);
            return View(movements);
        }
    }
}