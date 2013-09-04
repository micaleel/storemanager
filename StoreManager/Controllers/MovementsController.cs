using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;
using StoreManager.Repositories;
using System.IO;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class MovementsController : BaseController {
        private readonly MovementRepository _movementRepo;

        public MovementsController() {
            _movementRepo = new MovementRepository(Db);
        }

        public ActionResult DownloadImageFile(string fileName) {
            var dir = Server.MapPath("/App_Data/Pictures");
            var path = Path.Combine(dir, fileName);
            return base.File(path, "image/jpeg");
        }

        public ActionResult Index() {
            var movements = _movementRepo.All;    // TODO: Optimize
            return View(movements.ToList());
        }

        public ActionResult Details(int id = 0) {
            var movement = _movementRepo.Find(id);
            if (movement == null) return HttpNotFound("Cannot find movement with given ID");

            return View(movement);
        }

        public ActionResult Delete(int id = 0) {
            var movement = _movementRepo.Find(id);
            if (movement == null) return HttpNotFound("Cannot find movement with given ID");

            return View(movement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            try {
                _movementRepo.Delete(id);
            }
            catch (EntityNotFoundException ex) {
                ModelState.AddModelError("", ex.Message);
                FlashError(ex.Message);
            }

            return RedirectToAction("Index");
        }
    }
}