using System.Web;
using System.Web.Optimization;

namespace PhysicianPortal.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js"
                    //,
                    //"~/Scripts/jquery.unobtrusive-ajax.js"
                    )
                    );

           
            bundles.Add(new ScriptBundle("~/bundles/jquerySignalr").Include(
                    "~/Scripts/jquery.signalR-2.2.1.js",
                    "~/Scripts/ PhysicianPortalSignalR.js"));

            bundles.Add(new ScriptBundle("~/bundles/doubletap").Include(
                    "~/Scripts/DoubleTap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/Site.css",
                    "~/Content/jQueryNotifyBar/jquery.notifyBar.css"));

            bundles.Add(new StyleBundle("~/Content/UserCustomization").Include(
                    "~/Content/user-customization.css"));

            bundles.Add(new ScriptBundle("~/bundles/kendo")
                    .Include("~/Scripts/kendo/2017.1.118/jquery.min.js")
                    .Include("~/Scripts/kendo/2017.1.118/kendo.all.min.js")
                    .Include("~/Scripts/kendo/2017.1.118/kendo.aspnetmvc.min.js")
                    .Include("~/Scripts/kendo/2017.1.118/jszip.min.js")
                    .Include("~/Scripts/jQueryNotifyBar/jquery.notifyBar.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/2017.1.118/css")
                    .Include("~/Content/kendo/2017.1.118/kendo.common.min.css")
                    .Include("~/Content/kendo/2017.1.118/kendo.bootstrap.min.css")
                    .Include("~/Content/kendo/2017.1.118/kendo.bootstrap.mobile.min.css"));
            
            // Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
            bundles.IgnoreList.Clear();

            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}
