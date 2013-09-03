using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using StoreManager.Infrastructure;
using StoreManager.Models;

namespace StoreManager.Controllers {
    public abstract class BaseController : Controller {
        protected StoreManagerContext Db { get; private set; }

        protected BaseController() {
            Db = new StoreManagerContext();
            Mapper.AddProfile<StoreManagerProfile>();
        }

        protected override void OnAuthorization(AuthorizationContext filterContext) {
            if (filterContext.HttpContext.User == null) return;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) return;
            if (!(filterContext.HttpContext.User.Identity is FormsIdentity)) return;
            var id = filterContext.HttpContext.User.Identity as FormsIdentity;
            if (id == null) return;
            var ticket = id.Ticket;
            var userData = ticket.UserData;
            var userDateTokens = userData.Split('|');
            var roles = userDateTokens[0];

            filterContext.HttpContext.User = new GenericPrincipal(id, new[] { roles });
        }

        public string LoggedInUserName() {
            return User.Identity.Name;
        }

        public User LoggedInUser() {
            var userName = LoggedInUserName();
            return Db.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public void FlashInfo(string message) {
            TempData["ToastInfo"] = message;
        }

        public void FlashSuccess(string message) {
            TempData["ToastSuccess"] = message;
        }

        public void FlashError(string message) {
            TempData["ToastError"] = message;
        }

        public void FlashWarning(string message) {
            TempData["ToastWarning"] = message;
        }

        public void FlashDeleted(string descriptor) {
            FlashInfo(string.Format("The record '{0}' has been deleted successfully", descriptor));
        }

        public void FlashUpdated(string descriptor) {
            FlashInfo(string.Format("The record '{0}' has been updated successfully", descriptor));
        }

        public void FlashAdded(string descriptor) {
            FlashInfo(string.Format("The record '{0}' has been added successfully", descriptor));
        }
        protected override void Dispose(bool disposing) {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
