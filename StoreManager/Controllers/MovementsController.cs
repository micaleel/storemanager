using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Models;

namespace StoreManager.Controllers {


    [AuthorizeAndRedirect]
    public class MovementsController : BaseController {

        public ActionResult Index() {
            var movements = Db.Movements.Include(m => m.Item).Include(m => m.Location);
            return View(movements.ToList());
        }

        public ActionResult Details(int id = 0) {
            Movement movement = Db.Movements.Find(id);
            if (movement == null) {
                return HttpNotFound();
            }
            return View(movement);
        }

        public ActionResult Create() {
            ViewBag.ItemId = new SelectList(Db.Items, "Id", "Name");
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movement movement) {
            if (ModelState.IsValid) {
                Db.Movements.Add(movement);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(Db.Items, "Id", "Name", movement.ItemId);
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name", movement.LocationId);
            return View(movement);
        }

        public ActionResult Edit(int id = 0) {
            Movement movement = Db.Movements.Find(id);
            if (movement == null) {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(Db.Items, "Id", "Name", movement.ItemId);
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name", movement.LocationId);
            return View(movement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movement movement) {
            if (ModelState.IsValid) {
                Db.Entry(movement).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(Db.Items, "Id", "Name", movement.ItemId);
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name", movement.LocationId);
            return View(movement);
        }

        public ActionResult Delete(int id = 0) {
            Movement movement = Db.Movements.Find(id);
            if (movement == null) {
                return HttpNotFound();
            }
            return View(movement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Movement movement = Db.Movements.Find(id);
            Db.Movements.Remove(movement);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}