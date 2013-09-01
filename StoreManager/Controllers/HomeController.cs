using System.Web.Mvc;

namespace StoreManager.Controllers {

    public class HomeController : BaseController {

        public ActionResult Index() {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");
            return View();
        }

        [Authorize]
        public ActionResult Dashboard() {
            return View();
        }
    }
}
