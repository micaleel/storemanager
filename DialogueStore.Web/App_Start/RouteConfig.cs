using System.Web.Mvc;
using System.Web.Routing;

namespace DialogueStore.Web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("LoginRoute", "login", new { controller = "Account", action = "Login" });
            routes.MapRoute("RegisterRoute", "register", new { controller = "Account", action = "Register" });
            routes.MapRoute("ChangePasswordRoute", "changepassword", new { controller = "Account", action = "ChangePassword" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.LowercaseUrls = true;
        }
    }
}