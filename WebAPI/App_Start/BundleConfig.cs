using System.Web.Optimization;

namespace WebAPI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/scripts/jquery-{version}.js"));

//            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
//                        "~/Content/scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/scripts/bootstrap.min.js",
                      "~/Content/scripts/respond.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/site.min.css"));
        }
    }
}
