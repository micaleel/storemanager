using System.Data;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Models;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class LocationsController : BaseController {

        public ActionResult Index() {
            return View(Db.Locations.ToList());
        }

        public ActionResult Details(int id = 0) {
            Location location = Db.Locations.Find(id);
            if (location == null) {
                return HttpNotFound();
            }
            return View(location);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location) {
            if (ModelState.IsValid) {
                Db.Locations.Add(location);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        public ActionResult Edit(int id = 0) {
            Location location = Db.Locations.Find(id);
            if (location == null) {
                return HttpNotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location) {
            if (ModelState.IsValid) {
                Db.Entry(location).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        public ActionResult Delete(int id = 0) {
            Location location = Db.Locations.Find(id);
            if (location == null) {
                return HttpNotFound();
            }

            if (location.IsStore) {
                const string warningMsg =
                    "You cannot delete store location until another location is set as a primary location";
                ModelState.AddModelError("", warningMsg);
                FlashWarning(warningMsg);
            }

            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Location location = Db.Locations.Find(id);

            if (location.IsStore) {
                const string warningMsg =
                    "You cannot delete store location until another location is set as a primary location";
                ModelState.AddModelError("", warningMsg);
                FlashWarning(warningMsg);

                return View(location);
            }

            var movements = location.Movements.ToList();

            foreach (var movement in movements) {
                Db.Entry(movement).State = EntityState.Deleted;
            }

            Db.Locations.Remove(location);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Movements(int id) {
            var movements = Db.Movements.Where(m => m.LocationId == id).OrderByDescending(m => m.Date);
            return View(movements);
        }
    }
}