using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace StoreManager.App_Start {

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
                        "~/Content/bootstrap/bootstrap.min.css",
                        "~/Content/themes/base/jquery.ui.all.css",
                        "~/Content/Site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }

}