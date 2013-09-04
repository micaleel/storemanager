using System.Data;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Models;
using StoreManager.Repositories;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class ItemsController : BaseController {
        private readonly ItemRepository _itemRepo;

        public ItemsController() {
            _itemRepo = new ItemRepository(Db);
        }

        public ActionResult Index() {
            var items = _itemRepo.All.ToList();
            return View(items);
        }

        public ActionResult Details(int id = 0) {
            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            return View(item);
        }

        public ActionResult Create() {
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");
            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item) {
            if (!ModelState.IsValid) return View(item);

            _itemRepo.Add(item);

            FlashSuccess(string.Format("Item '{0}' has been added successfully", item.Name));

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id = 0) {
            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item) {
            if (!ModelState.IsValid) return View(item);

            _itemRepo.Update(item);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id = 0) {
            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            try {
                _itemRepo.Delete(id);
            }
            catch (EntityNotFoundException ex) {
                ModelState.AddModelError("", ex.Message);
                FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Movements(int id) {
            var movements = Db.Movements.Where(m => m.Stock.ItemId == id).OrderByDescending(m => m.DateCreated);
            return View(movements);
        }
    }
}