using System.Linq;
using System.Web.Mvc;
using DialogueStore.Web.Models;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Repositories;

namespace DialogueStore.Web.Controllers
{
    [AuthorizeAndRedirect]
    public class StockConditionsController : BaseController
    {
        private readonly StockConditionRepository _conditionRepo;

        public StockConditionsController()
        {
            _conditionRepo = new StockConditionRepository(Db);
        }

        public ActionResult Index()
        {
            return View(_conditionRepo.All.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockCondition condition)
        {
            if (!ModelState.IsValid) return View(condition);

            _conditionRepo.Add(condition);

            LogActivity("viewed details for condition ", condition.Name, condition.Id);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0)
        {
            var condition = _conditionRepo.Find(id);
            if (condition == null) return HttpNotFound("Cannot find Stock Condition with given ID");

            return View(condition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockCondition condition)
        {
            if (!ModelState.IsValid) return View(condition);

            _conditionRepo.Update(condition);

            LogActivity("modified stock condition record for ", condition.Name, condition.Id);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0)
        {
            var condition = _conditionRepo.Find(id);
            if (condition == null) return HttpNotFound("Cannot find Stock Condition with given ID");

            return View(condition);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var item = Db.StockConditions.Find(id);

                LogActivity("deleted stock condition record for ", item.Name, item.Id);

                _conditionRepo.Delete(id);
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public void LogActivity(string verb, string objectDescriptor, int? objectId)
        {
            LogActivity(verb, TimelineActivityObjectType.StockCondition, objectDescriptor, objectId);
        }
    }
}