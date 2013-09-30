using System.Linq;
using System.Web.Mvc;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Models;
using DialogueStore.Web.Repositories;

namespace DialogueStore.Web.Controllers
{
    [AuthorizeAndRedirect]
    public class ItemsController : BaseController
    {
        private readonly ItemRepository _itemRepo;

        public ItemsController()
        {
            _itemRepo = new ItemRepository(Db);
        }

        public ActionResult Index()
        {
            var items = _itemRepo.All.ToList();
            return View(items);
        }

        public ActionResult Details(int id = 0)
        {
            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            LogActivity("viewed details for item ", item.Name, item.Id);

            return View(item);
        }

        public ActionResult Create()
        {
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            if (!ModelState.IsValid) return View(item);

            _itemRepo.Add(item);

            LogActivity("created item record for ", item.Name, item.Id);

            FlashSuccess(string.Format("Item '{0}' has been added successfully", item.Name));

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0)
        {
            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (!ModelState.IsValid) return View(item);

            _itemRepo.Update(item);

            LogActivity("modified item record for ", item.Name, item.Id);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0)
        {
            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var item = Db.Items.Find(id);

                LogActivity("deleted item record for ", item.Name, item.Id);

                _itemRepo.Delete(id);
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
            LogActivity(verb, TimelineActivityObjectType.Item, objectDescriptor, objectId);
        }
    }
}