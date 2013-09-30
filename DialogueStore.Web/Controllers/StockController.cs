using System.Web;
using System.Web.Mvc;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Models;
using DialogueStore.Web.Views.Stock;
using System;
using DialogueStore.Web.Repositories;

namespace DialogueStore.Web.Controllers
{
    [AuthorizeAndRedirect]
    public class StockController : BaseController
    {
        private readonly IFileSaver _pictureSaver = new PictureSaver();
        private readonly ItemRepository _itemRepo;
        private readonly StockRepository _stockRepo;

        public StockController()
        {
            _stockRepo = new StockRepository(Db);
            _itemRepo = new ItemRepository(Db);
        }

        public ActionResult Create(int id)
        {
            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name");

            var item = _itemRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find item with given ID");

            var stockModel = CreateStockModel.Create(item);

            return View(stockModel);
        }

        public ActionResult Details(int id = 0)
        {
            var item = _stockRepo.Find(id);
            if (item == null) return HttpNotFound("Cannot find Item with given ID");

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateStockModel input)
        {
            var item = _itemRepo.Find(input.ItemId);
            if (item == null) return HttpNotFound("Cannot find item with specified itemId");

            if (ModelState.IsValid)
            {
                _itemRepo.AddStocks(item.Id, CreateStockModel.Explode(input));

                LogActivity(string.Format("added {0} stock items to ", input.Quantity), 
                    TimelineActivityObjectType.Item, item.Name, item.Id);

                FlashSuccess(string.Format("added {0} stock items to {1}", input.Quantity, item.Name));

                return RedirectToAction("Details", "Items", new { id = input.ItemId });
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", input.StockConditionId);

            input = new CreateStockModel { ItemId = input.ItemId, Item = item };

            return View(input);
        }

        public ActionResult Edit(int id = 0)
        {
            var item = _stockRepo.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", item.StockConditionId);

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stock stock, bool batchUpdate = false)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.StockConditionId = new SelectList(Db.StockConditions, "Id", "Name", stock.StockConditionId);
                return View(stock);
            }

            _stockRepo.Update(stock, batchUpdate);

            var stockItem = Db.Stocks.Find(stock.Id);

            LogActivity("updated stock quantity for ", stockItem.Item.Name, stockItem.ItemId);

            return RedirectToAction("Details", "Items", new { id = stock.ItemId });
        }

        public ActionResult Delete(int id = 0)
        {
            var stock = _stockRepo.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with given ID");

            return View(stock);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, bool batchUpdate = false)
        {
            var stock = _stockRepo.Find(id);

            if (stock == null) return HttpNotFound("Cannot find Stock with given ID");

            var itemId = stock.ItemId;
            var itemName = stock.Item.Name;

            if (batchUpdate)
            {
                _stockRepo.DeleteByBatchId(stock.BatchId);
                LogActivity("deleted multiple stock items for ", itemName, itemId);
            }
            else
            {
                _stockRepo.Delete(stock.Id);
                LogActivity("deleted stock item for ", itemName, itemId);
            }


            Db.SaveChanges();

            return RedirectToAction("Details", "Items", new { id = itemId });
        }

        public ActionResult Move(int id)
        {
            var stock = _stockRepo.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with specified ID");

            ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");

            var viewModel = MoveStockModel.Create(stock);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(MoveStockModel input, HttpPostedFileBase requisitionDoc, HttpPostedFileBase authorizationDoc)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LocationId = new SelectList(Db.Locations, "Id", "Name");
                return View(input);
            }

            var stock = _stockRepo.Find(input.StockId);
            if (stock == null) return new HttpNotFoundResult("Cannot find Stock with given ID");

            var movement = new Movement
            {
                DateCreated = DateTime.UtcNow,
                StockId = input.StockId,
                ToLocationId = input.LocationId,
                Notes = input.Notes,
            };

            movement = _itemRepo.MoveStock(movement);

            var movementRepo = new MovementRepository(Db);

            if (requisitionDoc != null)
            {
                movementRepo.AttachRequisitionDoc(movement.Id, _pictureSaver.Save(requisitionDoc));
            }

            if (authorizationDoc != null)
            {
                movementRepo.AttachAuthorizationDoc(movement.Id, _pictureSaver.Save(authorizationDoc));
            }
            var toLocation = Db.Locations.Find(input.LocationId);
            if (toLocation != null)
            {
                LogActivity("moved ", movement.ToString(), movement.Id);

                FlashSuccess(string.Format("Stock item '{0}' has been moved to the location '{1}' successfully", stock.Item.Name,
                                           toLocation.Name));
            }
            return RedirectToAction("Details", "Items", new { id = stock.ItemId });
        }

        public void LogActivity(string verb, string objectDescriptor, int? objectId)
        {
            LogActivity(verb, TimelineActivityObjectType.Stock, objectDescriptor, objectId);
        }
    }
}