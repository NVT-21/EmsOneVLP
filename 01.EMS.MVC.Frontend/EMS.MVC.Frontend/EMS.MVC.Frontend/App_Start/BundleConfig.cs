using System.Web;
using System.Web.Optimization;

namespace EMS.MVC.Frontend
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region SAMPLE
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            #endregion

            #region CSS
            //Cách 1: nhiều tập tin trong thư mục
            //bundles.Add(new StyleBundle("~/Assets/css").Include(
            //          "~/Assets/css/style.css",
            //          "~/Assets/css/main.css"));

            //Cách 2: tất cả tập tin trong thư mục
            //bundles.Add(new StyleBundle("~/bundles/css").IncludeDirectory("~/Assets/css/", "*.css", true));

            //Cách 3: từng tập tin trong thư mục và đưỡng dẫn tuyệt đối
            //bundles.Add(new StyleBundle("~/bundles/css").Include(
            //                    "~/Assets/css/main.css",
            //                    new CssRewriteUrlTransform()
            //                ));

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Assets/vendors/jquery-nice-select/css/nice-select.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/select2/css/select2.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/daterangepicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/jquery-ui-1.13.2/jquery-ui.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/bootstrap-select-1.14.0-beta2/dist/css/bootstrap-select.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/dataTables/datatables.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/jspreadsheet/jspreadsheet.css")
                .Include("~/Assets/vendors/jspreadsheet/jsuites.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/jstree/dist/themes/default/style.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/icons/font-awesome/css/all.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/icons/themify-icons/css/themify-icons.css", new CssRewriteUrlTransform())
                .Include("~/Assets/css/LiveChat.css", new CssRewriteUrlTransform())
                .Include("~/Assets/css/style.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/bootstrap-datepicker/bootstrap-datepicker.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/vendors/datetimepicker-master/jquery.datetimepicker.min.css", new CssRewriteUrlTransform())
                .Include("~/Assets/css/main.css"));

            bundles.UseCdn = true;   //enable CDN support

            bundles.Add(new StyleBundle("~/bundles/fonts", "https://fonts.googleapis.com/css2?family=Inter:wght@200;300;400;500;600;700;900&display=swap"));
            #endregion

            #region JAVASCRIPT
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                              "~/Assets/vendors/jquery-nice-select/js/jquery.nice-select.min.js",
                              "~/Assets/vendors/select2/js/select2.full.min.js",
                              "~/Assets/vendors/moment/moment.min.js",
                              "~/Assets/vendors/daterangepicker/daterangepicker.js",
                              "~/Assets/vendors/sweetalert/sweetalert.min.js",
                              "~/Assets/vendors/jquery-ui-1.13.2/jquery-ui.js",
                              "~/Assets/vendors/bootstrap-select-1.14.0-beta2/dist/js/bootstrap-select.min.js",
                              "~/Assets/vendors/bootstrap-select-1.14.0-beta2/dist/js/i18n/defaults-vi_VN.min.js",
                              "~/Assets/vendors/dataTables/datatables.min.js",
                              "~/Assets/vendors/jspreadsheet/jspreadsheet.js",
                              "~/Assets/vendors/jspreadsheet/jsuites.js",
                              "~/Assets/vendors/jspreadsheet/syncInput.min.js",
                              "~/Assets/vendors/chart/Chart.bundle.min.js",
                              "~/Assets/vendors/inputmask/jquery.inputmask.bundle.js",
                              "~/Assets/vendors/jstree/dist/jstree.min.js",
                              "~/Assets/vendors/SignalR/jquery.signalR-2.4.3.js",
                              "~/Assets/js/custom.js",
                              "~/Assets/js/deznav-init.js",
                              "~/Assets/js/string.js",
                              "~/Assets/vendors/bootstrap-datepicker/bootstrap-datepicker.min.js"));


            //the following creates bundles in debug mode;
            //BundleTable.EnableOptimizations = true;
            #endregion

        }
    }
}
