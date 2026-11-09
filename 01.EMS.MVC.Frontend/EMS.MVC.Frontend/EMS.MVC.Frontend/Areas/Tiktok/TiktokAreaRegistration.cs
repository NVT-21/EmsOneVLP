using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Tiktok
{
    public class TiktokAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Tiktok";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            #region TIK_HOMECONTROLLER
            context.MapRoute(
                name: "Tiktok/Trang-Chu",
                url: "Tiktok/Trang-Chu",
                defaults: new { controller = "TIK_Home", action = "TIK_DashboardView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Tiktok.Controllers" });
            #endregion

            #region TIK_PRODUCT_CONTROLLER
            context.MapRoute(
                name: "Tiktok/San-Pham",
                url: "Tiktok/San-Pham",
                defaults: new { controller = "TIK_Product", action = "TIK_ProductView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Tiktok.Controllers" });
            #endregion

            #region TIK_ORDER_CONTROLLER
            context.MapRoute(
                name: "Tiktok/Don-Hang",
                url: "Tiktok/Don-Hang",
                defaults: new { controller = "TIK_Order", action = "TIK_OrderView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Tiktok.Controllers" });
            #endregion

            #region TIK_CONFIGURATION
            context.MapRoute(
                name: "Tiktok/Cau-Hinh",
                url: "Tiktok/Cau-Hinh",
                defaults: new { controller = "TIK_Configuration", action = "TIK_ConfigurationView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Tiktok.Controllers" });
            #endregion

            context.MapRoute(
                "Tiktok_default",
                "Tiktok/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}