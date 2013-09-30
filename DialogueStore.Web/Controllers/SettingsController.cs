using System.Web.Mvc;
using DialogueStore.Web.Infrastructure;

namespace DialogueStore.Web.Controllers
{
    [AuthorizeAndRedirect(Roles = "Admin")]
    public class SettingsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
