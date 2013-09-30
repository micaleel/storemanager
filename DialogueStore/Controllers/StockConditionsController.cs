using System.Data;
using System.Linq;
using System.Web.Mvc;
using DialogueStore.Models;
using DialogueStore.Infrastructure;
using DialogueStore.Repositories;

namespace DialogueStore.Controllers {

    [AuthorizeAndRedirect]
    public class StockConditionsController : BaseController {
        private StockConditionRepository conditionRepo;

        public StockConditionsController() {
            conditionRepo = new StockConditionRepository(Db);
        }

        public ActionResult Index() {
            return View(conditionRepo.All.ToList());
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockCondition condition) {
            if (!ModelState.IsValid) return View(condition);

            conditionRepo.Add(condition);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0) {
            var condition = conditionRepo.Find(id);
            if (condition == null) return HttpNotFound("Cannot find Stock Condition with given ID");

            return View(condition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockCondition condition) {
            if (!ModelState.IsValid)return View(condition);

            conditionRepo.Update(condition);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0) {
            var condition = conditionRepo.Find(id);
            if (condition == null) return HttpNotFound("Cannot find Stock Condition with given ID");

            return View(condition);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            try {
                conditionRepo.Delete(id);
            }
            catch (EntityNotFoundException ex) {
                ModelState.AddModelError("", ex.Message);
                FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}