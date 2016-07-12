using System;
using System.Web.Optimization;

namespace Inscoo
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            /*登陆页面*/
            bundles.Add(new ScriptBundle("~/bundles/index").Include(
                       "~/Scripts/jquery-{version}.js",
                       "~/Scripts/index.js",
                       "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/index").Include(
                    "~/Content/index.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery-migrate*",
                       "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryAjax").Include(
                      "~/Scripts/jquery.unobtrusive-ajax.js",
                      "~/Scripts/jquery.validate.unobtrusive.js"));
            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Content/js/bootstrap-datepicker.js",
                      "~/Content/js/bootstrap-datepicker.zh-CN.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                 "~/Scripts/jquery.unobtrusive*",
                      "~/Scripts/base.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/base.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                 "~/Content/css/bootstrap-datepicker.css",
                 "~/Content/bootstrap-theme.css",
                    "~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/FontAwesome").Include(
                    "~/Content/font-awesome.css"));

        }
    }
}