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
    public class MovementsController : Controller
    {
        private StoreManagerContext db = new StoreManagerContext();

        //
        // GET: /Movements/

        public ActionResult Index()
        {
            var movements = db.Movements.Include(m => m.Item).Include(m => m.Location);
            return View(movements.ToList());
        }

        //
        // GET: /Movements/Details/5

        public ActionResult Details(int id = 0)
        {
            Movement movement = db.Movements.Find(id);
            if (movement == null)
            {
                return HttpNotFound();
            }
            return View(movement);
        }

        //
        // GET: /Movements/Create

        public ActionResult Create()
        {
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name");
            ViewBag.ItemStatusId = new SelectList(db.ItemStatuses, "Id", "Name");
            return View();
        }

        //
        // POST: /Movements/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movement movement)
        {
            if (ModelState.IsValid)
            {
                db.Movements.Add(movement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", movement.ItemId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", movement.LocationId);
            return View(movement);
        }

        //
        // GET: /Movements/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Movement movement = db.Movements.Find(id);
            if (movement == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", movement.ItemId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", movement.LocationId);
            return View(movement);
        }

        //
        // POST: /Movements/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movement movement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", movement.ItemId);
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Name", movement.LocationId);
            return View(movement);
        }

        //
        // GET: /Movements/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Movement movement = db.Movements.Find(id);
            if (movement == null)
            {
                return HttpNotFound();
            }
            return View(movement);
        }

        //
        // POST: /Movements/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movement movement = db.Movements.Find(id);
            db.Movements.Remove(movement);
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