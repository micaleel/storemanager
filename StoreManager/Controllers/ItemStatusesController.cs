using System.Data;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Models;

namespace StoreManager.Controllers
{
    public class ItemStatusesController : Controller
    {
        private StoreManagerContext db = new StoreManagerContext();

        //
        // GET: /ItemStatuses/

        public ActionResult Index()
        {
            return View(db.ItemStatuses.ToList());
        }

        //
        // GET: /ItemStatuses/Details/5

        public ActionResult Details(int id = 0)
        {
            ItemStatus itemstatus = db.ItemStatuses.Find(id);
            if (itemstatus == null)
            {
                return HttpNotFound();
            }
            return View(itemstatus);
        }

        //
        // GET: /ItemStatuses/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ItemStatuses/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemStatus itemstatus)
        {
            if (ModelState.IsValid)
            {
                db.ItemStatuses.Add(itemstatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(itemstatus);
        }

        //
        // GET: /ItemStatuses/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ItemStatus itemstatus = db.ItemStatuses.Find(id);
            if (itemstatus == null)
            {
                return HttpNotFound();
            }
            return View(itemstatus);
        }

        //
        // POST: /ItemStatuses/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemStatus itemstatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemstatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemstatus);
        }

        //
        // GET: /ItemStatuses/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ItemStatus itemstatus = db.ItemStatuses.Find(id);
            if (itemstatus == null)
            {
                return HttpNotFound();
            }
            return View(itemstatus);
        }

        //
        // POST: /ItemStatuses/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemStatus itemstatus = db.ItemStatuses.Find(id);
            db.ItemStatuses.Remove(itemstatus);
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