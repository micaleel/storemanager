using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Models;

namespace StoreManager.Controllers
{
    public class ItemsController : Controller
    {
        private StoreManagerContext db = new StoreManagerContext();

        //
        // GET: /Items/

        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.Status);
            return View(items.ToList());
        }

        //
        // GET: /Items/Details/5

        public ActionResult Details(int id = 0)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // GET: /Items/Create

        public ActionResult Create()
        {
            ViewBag.ItemStatusId = new SelectList(db.ItemStatuses, "Id", "Name");
            return View();
        }

        //
        // POST: /Items/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemStatusId = new SelectList(db.ItemStatuses, "Id", "Name", item.ItemStatusId);
            return View(item);
        }

        //
        // GET: /Items/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemStatusId = new SelectList(db.ItemStatuses, "Id", "Name", item.ItemStatusId);
            return View(item);
        }

        //
        // POST: /Items/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemStatusId = new SelectList(db.ItemStatuses, "Id", "Name", item.ItemStatusId);
            return View(item);
        }

        //
        // GET: /Items/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        //
        // POST: /Items/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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