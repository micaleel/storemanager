using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DialogueStore.Infrastructure;
using DialogueStore.Models;
using DialogueStore.Repositories;

namespace DialogueStore.Controllers {

    [AuthorizeAndRedirect]
    public class LocationsController : BaseController {
        private readonly LocationRepository _locationRepo;

        public LocationsController() {
            _locationRepo = new LocationRepository(Db);
        }

        public ActionResult Index() {
            return View(_locationRepo.All.ToList());
        }

        public ActionResult Details(int id = 0) {
            Location location = _locationRepo.Find(id);
            if (location == null) return HttpNotFound("Cannot find Location with given ID");

            return View(location);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location) {
            if (!ModelState.IsValid) return View(location);

            _locationRepo.Add(location);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0) {
            var location = _locationRepo.Find(id);
            if (location == null) return HttpNotFound("Cannot find Location with given ID");

            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location) {
            if (!ModelState.IsValid) return View(location);

            _locationRepo.Update(location);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0) {
            var location = _locationRepo.Find(id);
            if (location == null) return HttpNotFound("Cannot find Location with given ID");


            if (location.IsStore) {
                const string warningMsg =
                    "Cannot delete store location until another location is set as a primary location";
                ModelState.AddModelError("", warningMsg);
                FlashWarning(warningMsg);
            }

            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            try {
                _locationRepo.Delete(id);
            }
            catch (ApplicationException ex) {
                ModelState.AddModelError("", ex.Message);
                FlashWarning(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public PartialViewResult LocationInventory(int id) {
            var stocks = Db.Stocks.Where(x => x.LocationId == id).ToList();
            return PartialView("_LocationInventory", stocks);
        }
    }
}