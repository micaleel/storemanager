using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Models;
using DialogueStore.Web.Repositories;

namespace DialogueStore.Web.Controllers
{
    [AuthorizeAndRedirect]
    public class LocationsController : BaseController
    {
        private readonly LocationRepository _locationRepo;

        public LocationsController()
        {
            _locationRepo = new LocationRepository(Db);
        }

        public ActionResult Index()
        {
            return View(_locationRepo.All.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Location location = _locationRepo.Find(id);
            if (location == null) return HttpNotFound("Cannot find Location with given ID");

            LogActivity("viewed details for location ", location.Name, location.Id);

            return View(location);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location)
        {
            if (!ModelState.IsValid) return View(location);

            _locationRepo.Add(location);

            LogActivity("created location record for ", location.Name, location.Id);

            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0)
        {
            var location = _locationRepo.Find(id);
            if (location == null) return HttpNotFound("Cannot find Location with given ID");

            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location)
        {
            if (!ModelState.IsValid) return View(location);

            _locationRepo.Update(location);

            LogActivity("modified location record for ", location.Name, location.Id);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0)
        {
            var location = _locationRepo.Find(id);
            if (location == null) return HttpNotFound("Cannot find Location with given ID");


            if (location.IsStore)
            {
                const string warningMsg =
                    "Cannot delete store location until another location is set as a primary location";
                ModelState.AddModelError("", warningMsg);
                FlashWarning(warningMsg);
            }

            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var item = Db.Locations.Find(id);

                LogActivity("deleted location record for ", item.Name, item.Id);

                _locationRepo.Delete(id);
            }
            catch (ApplicationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                FlashWarning(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public PartialViewResult LocationInventory(int id)
        {
            var stocks = Db.Stocks.Where(x => x.LocationId == id).ToList();
            return PartialView("_LocationInventory", stocks);
        }

        public void LogActivity(string verb, string objectDescriptor, int? objectId)
        {
            LogActivity(verb, TimelineActivityObjectType.Location, objectDescriptor, objectId);
        }
    }
}