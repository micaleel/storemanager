using System.Web.Optimization;

namespace DialogueStore.App_Start {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.autosize.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/Site.js"));

            bundles.Add(new StyleBundle("~/Content/styles").Include(
                        "~/Content/bootstrap/bootstrap.css",
                        "~/Content/bootstrap/bootstrap-theme.css",
                        "~/Content/themes/base/jquery.ui.all.css",
                        "~/Content/Site.css"));
        }
    }

}