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
            var movements = Db.Movements.Include(m => m.Stock).Include(m => m.Location);
            return View(movements.ToList());
        }

        public ActionResult Details(int id = 0) {
            Movement movement = Db.Movements.Find(id);
            if (movement == null) {
                return HttpNotFound();
            }
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