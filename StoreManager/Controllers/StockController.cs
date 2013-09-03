using System.Web;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Models;
using StoreManager.Views.Stock;
using System;
using StoreManager.Repositories;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class StockController : BaseController {
        private readonly IFileSaver _pictureSaver = new PictureSaver();
        private readonly ItemRepository _itemRepo;
        private readonly StockRepository _stockRepo;

        public StockController() {
            _stockRepo = new StockRepository(Db);
            _itemRepo = new ItemRepository(Db);
        }

        public ActionResult Create(int id) {
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");

            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find item with given ID");

            var stockModel = CreateStockModel.Create(item);

            return View(stockModel);
        }

        public ActionResult Details(int id = 0) {
            var item = _stockRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateStockModel input) {
            var item = _itemRepo.Find(input.ItemId);
            if (item == null) return HttpNotFound("Cannot find item with specified itemId");

            if (ModelState.IsValid) {
                _itemRepo.AddStocks(item.Id, CreateStockModel.Explode(input));

                FlashSuccess(string.Format("{0} stock items have been added to {1}", input.Quantity, item.Name));

                return RedirectToAction("Details", "Items", new { id = input.ItemId });
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", input.StockConditionId);

            input = new CreateStockModel { ItemId = input.ItemId, Item = item };

            return View(input);
        }

        public ActionResult Edit(int id = 0) {
            var item = _stockRepo.Find(id);
            if (item == null) {
                return HttpNotFound();
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", item.StockConditionId);

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stock stock, bool batchUpdate = false) {
            if (!ModelState.IsValid) {
                ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", stock.StockConditionId);
                return View(stock);
            }

            _stockRepo.Update(stock, batchUpdate);

            return RedirectToAction("Details", "Items", new { id = stock.ItemId });
        }

        public ActionResult Delete(int id = 0) {
            var stock = _stockRepo.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with given ID");

            return View(stock);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, bool batchUpdate = false) {
            var stock = _stockRepo.Find(id);

            if (stock == null) return HttpNotFound("Cannot find Stock with given ID");

            var itemId = stock.ItemId;

            if (batchUpdate) {
                _stockRepo.DeleteByBatchId(stock.BatchId);
            }
            else {
                _stockRepo.Delete(stock.Id);
            }

            Db.SaveChanges();

            return RedirectToAction("Details", "Items", new { id = itemId });
        }

        public ActionResult Move(int id) {
            var stock = _stockRepo.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with specified ID");

            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");

            var viewModel = MoveStockModel.Create(stock);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(MoveStockModel input, HttpPostedFileBase requisitionDoc, HttpPostedFileBase authorizationDoc) {
            if (!ModelState.IsValid) {
                ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");
                return View(input);
            }

            var stock = _stockRepo.Find(input.StockId);
            if (stock == null) return new HttpNotFoundResult("Cannot find Stock with given ID");

            var movement = new Movement {
                DateCreated = DateTime.UtcNow,
                StockId = input.StockId,
                LocationId = input.LocationId,
                Notes = input.Notes,
            };

            movement = _itemRepo.MoveStock(movement);

            var movementRepo = new MovementRepository(Db);

            if (requisitionDoc != null) {
                movementRepo.AttachRequisitionDoc(movement.Id, _pictureSaver.Save(requisitionDoc));
            }

            if (authorizationDoc != null) {
                movementRepo.AttachAuthorizationDoc(movement.Id, _pictureSaver.Save(authorizationDoc));
            }

            return RedirectToAction("Details", "Items", new { id = stock.ItemId });
        }
    }
}