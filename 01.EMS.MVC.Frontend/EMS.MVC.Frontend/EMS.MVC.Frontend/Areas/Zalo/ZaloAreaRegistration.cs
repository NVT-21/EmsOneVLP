using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Zalo
{
    public class ZaloAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Zalo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            #region ZAL_HOMECONTROLLER
            context.MapRoute(
                name: "zalo/trang-chu",
                url: "zalo/trang-chu",
                defaults: new { controller = "ZAL_Home", action = "ZAL_DashboardView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Zalo.Controllers" });
            #endregion

            #region ZAL_LIVE_CHAT_CONTROLLER
            context.MapRoute(
                name: "zalo/live-chat",
                url: "zalo/live-chat",
                defaults: new { controller = "ZAL_LiveChat", action = "ZAL_LiveChatView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Zalo.Controllers" });
            #endregion

            #region ZAL_FOLLOWER_CONTROLLER
            context.MapRoute(
                name: "zalo/khach-hang-tiem-nang",
                url: "zalo/khach-hang-tiem-nang",
                defaults: new { controller = "ZAL_Follower", action = "ZAL_FollowerView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Zalo.Controllers" });
            #endregion

            context.MapRoute(
                "Zalo_default",
                "Zalo/{controller}/{action}/{id}",
                new { controller = "ZAL_Home", action = "DashboardView", id = UrlParameter.Optional },
                new string[] { "EMS.MVC.Frontend.Areas.Zalo.Controllers" }
            );
        }
    }
}