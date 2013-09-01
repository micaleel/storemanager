using System;
using System.Linq;
using System.Web.Mvc;
using StoreManager.Infrastructure;

namespace StoreManager.Controllers {

    [AuthorizeAndRedirect]
    public class ReportsController : BaseController {

        public ActionResult Index() {
            return View();
        }

        public ActionResult RecentMovements() {
            var movements = Db.Movements.OrderBy(x => x.Date).Take(20);
            return View(movements);
        }

        public ActionResult Inventory() {
            var items = Db.Items.ToList();
            return View(items);
        }
    }
}
