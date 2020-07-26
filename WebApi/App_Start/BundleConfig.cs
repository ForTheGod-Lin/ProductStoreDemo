using System.Web;
using System.Web.Optimization;

namespace WebApi
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"
                ));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/EasyUICss").Include(
                "~/Content/jquery-easyui-1.8.6/themes/bootstrap/easyui.css",
                "~/Content/jquery-easyui-1.8.6/themes/icon.css",
                "~/Content/jquery-easyui-1.8.6/themes/color.css",
                "~/Content/jquery-easyui-1.8.6/demo/demo.css"));

            bundles.Add(new ScriptBundle("~/bundles/EasyUI").Include(
                "~/Content/jquery-easyui-1.8.6/jquery.min.js",
                "~/Content/jquery-easyui-1.8.6/jquery.easyui.min.js",
                "~/Content/jquery-easyui-1.8.6/locale/easyui-lang-zh_CN.js"));
        }
    }
   
}
