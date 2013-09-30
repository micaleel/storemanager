using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web;
using DialogueStore.Web.App_Start;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Models;

namespace DialogueStore.Web
{
    public class DialogueStoreApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer(new DialogueStoreContextInitializer());

            LoadSettings();
        }

        public static DialogueStoreSettings LoadSettings()
        {
            var settingsProvider = new DialogueSettingsProvider();
            var appSettings = settingsProvider.Load();
            settingsProvider.Save(appSettings);

            HttpContext.Current.Application["AppSettings"] = appSettings;

            return appSettings;
        }
    }
}