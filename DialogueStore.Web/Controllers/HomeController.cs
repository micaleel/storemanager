using System.Linq;
using System.Web.Mvc;
using DialogueStore.Web.ViewModels;

namespace DialogueStore.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            return View(Db.TimelineActivities.OrderByDescending(x => x.Date).ToList());
        }

        public ActionResult Search(string q)
        {
            var viewModel = new SearchResultModel
            {
                Items = Db.Items.Where(x => x.Name == q)
            };

            return View(viewModel);
        }
    }
}
