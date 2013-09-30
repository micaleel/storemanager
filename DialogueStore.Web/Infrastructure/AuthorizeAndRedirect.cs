using System.Web.Mvc;

namespace DialogueStore.Web.Infrastructure {

    public class AuthorizeAndRedirectAttribute : AuthorizeAttribute {

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
                var acceptedTypes = filterContext.HttpContext.Request.AcceptTypes;
                if (acceptedTypes != null)
                    foreach (var type in acceptedTypes) {
                        if (type.Contains("html")) {
                            filterContext.Result = filterContext.HttpContext.Request.IsAjaxRequest()
                                ? new ViewResult { ViewName = "AccessDeniedPartial" }
                                : new ViewResult { ViewName = "AccessDenied" };
                            break;
                        }
                        if (type.Contains("javascript")) {
                            filterContext.Result = new JsonResult { Data = new { success = false, message = "Access denied." } };
                            break;
                        }
                        if (type.Contains("xml")) {
                            filterContext.Result = new HttpUnauthorizedResult(); //this will redirect to login page with forms auth you could instead serialize a custom xml payload and return here.
                        }
                    }
            }
            else {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}