using System.Data;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Models;
using StoreManager.Infrastructure;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class StockConditionsController : BaseController {

        public ActionResult Index() {
            return View(Db.StockConditions.ToList());
        }

        public ActionResult Details(int id = 0) {
            StockCondition itemcondition = Db.StockConditions.Find(id);
            if (itemcondition == null) {
                return HttpNotFound();
            }
            return View(itemcondition);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockCondition itemcondition) {
            if (ModelState.IsValid) {
                Db.StockConditions.Add(itemcondition);
                Db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(itemcondition);
        }

        public ActionResult Edit(int id = 0) {
            StockCondition itemcondition = Db.StockConditions.Find(id);
            if (itemcondition == null) {
                return HttpNotFound();
            }
            return View(itemcondition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockCondition itemcondition) {
            if (ModelState.IsValid) {
                Db.Entry(itemcondition).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemcondition);
        }

        public ActionResult Delete(int id = 0) {
            StockCondition itemcondition = Db.StockConditions.Find(id);
            if (itemcondition == null) {
                return HttpNotFound();
            }
            return View(itemcondition);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            StockCondition itemcondition = Db.StockConditions.Find(id);
            Db.StockConditions.Remove(itemcondition);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}