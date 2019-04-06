using System.Web;
using System.Web.Optimization;

namespace ProductSearch
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                 "~/Scripts/bootstrap.js",
                 "~/Scripts/respond.js",
                 "~/Content/material/assets/js/jquery-3.1.1.min.js",
                 "~/Content/material/assets/js/jquery-ui.min.js",
                 "~/Content/material/assets/js/bootstrap.min.js",
                 "~/Content/material/assets/js/material.min.js",
                 "~/Content/material/assets/js/perfect-scrollbar.jquery.min.js",
                 "~/Content/material/assets/js/jquery.datatables.js",
                 "~/Content/material/assets/js/material-dashboard.js",
                 "~/Content/material/assets/js/demo.js"));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
