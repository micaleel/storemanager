using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManager.Models;

namespace StoreManager.Controllers
{
    public class LocationsController : Controller
    {
        private StoreManagerContext db = new StoreManagerContext();

        //
        // GET: /Locations/

        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        //
        // GET: /Locations/Details/5

        public ActionResult Details(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // GET: /Locations/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Locations/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        //
        // GET: /Locations/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Locations/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        //
        // GET: /Locations/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Locations/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}