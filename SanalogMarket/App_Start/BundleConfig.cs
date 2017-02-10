using System.Web;
using System.Web.Optimization;

namespace SanalogMarket
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/Content/AdminJs").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery-2.2.3.min.js",
                "~/Scripts/app.min.js"
                , "~/Scripts/demo.js"));


            bundles.Add(new StyleBundle("~/Content/AdminCss").Include(
                "~/Content/bootstrap.css",
                "~/Content/AdminLTE.css",
                "~/Content/AdminLTE.min.css",
                "~/Content/skins/skin-*"));
        }
    }
}