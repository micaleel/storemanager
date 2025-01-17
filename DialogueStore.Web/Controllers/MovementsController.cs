﻿using System;
using System.Linq;
using System.Web.Mvc;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Models;
using DialogueStore.Web.Repositories;
using System.IO;

namespace DialogueStore.Web.Controllers
{
    [AuthorizeAndRedirect]
    public class MovementsController : BaseController
    {
        private readonly MovementRepository _movementRepo;

        public MovementsController()
        {
            _movementRepo = new MovementRepository(Db);
        }

        public ActionResult DownloadImageFile(string fileName)
        {
            var dir = Server.MapPath("/App_Data/Pictures");
            var path = Path.Combine(dir, fileName);
            return base.File(path, "image/jpeg");
        }

        public ActionResult Index()
        {
            var movements = _movementRepo.All;    // TODO: Optimize
            return View(movements.ToList());
        }

        public ActionResult Details(int id = 0)
        {
            var movement = _movementRepo.Find(id);
            if (movement == null) return HttpNotFound("Cannot find movement with given ID");

            const string format = "stock item {0} from {1} to {2}";
            var message = string.Format(format,
                movement.Stock.Item.Name, 
                movement.FromLocation.Name,
                movement.ToLocation.Name);

            LogActivity("moved ", message, movement.Id);

            return View(movement);
        }

        public ActionResult Delete(int id = 0)
        {
            var movement = _movementRepo.Find(id);
            if (movement == null) return HttpNotFound("Cannot find movement with given ID");

            return View(movement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var item = Db.Movements.Find(id);

                LogActivity("deleted movement record for ", item.ToString(), item.Id);

                _movementRepo.Delete(id);
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ReturnToStore(int id)
        {
            var stockRepo = new StockRepository(Db);
            var stock = stockRepo.Find(id);
            if (stock == null) return HttpNotFound("Cannot find Stock with given ID");

            var locationRepo = new LocationRepository(Db);
            var storeLocation = locationRepo.GetOrCreateStoreLocation();

            var movement = new Movement
            {
                DateCreated = DateTime.UtcNow,
                StockId = id,
                ToLocationId = storeLocation.Id,
            };

            var itemRepo = new ItemRepository(Db);
            itemRepo.MoveStock(movement);

            LogActivity(string.Format("Returned item {0} to Store", stock.Item.Name),string.Empty, movement.Id);

            FlashSuccess(string.Format("Stock item '{0}' has been moved to the location '{1}' successfully", stock.Item.Name,
                                       storeLocation.Name));
            return RedirectToAction("Details", "Items", new { id = stock.ItemId });
        }


        public void LogActivity(string verb, string objectDescriptor, int? objectId)
        {
            LogActivity(verb, TimelineActivityObjectType.Movement, objectDescriptor, objectId);
        }
    }
}