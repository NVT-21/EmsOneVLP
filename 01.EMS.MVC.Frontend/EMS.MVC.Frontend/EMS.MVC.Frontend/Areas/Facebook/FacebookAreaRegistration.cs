using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Facebook
{
    public class FacebookAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Facebook";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            #region FAB_HOMECONTROLLER
            context.MapRoute(
                name: "Facebook/Trang-Chu",
                url: "Facebook/Trang-Chu",
                defaults: new { controller = "FAB_Home", action = "FAB_DashboardView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Facebook.Controllers" });
            #endregion

            #region FAB_LIVECHAT
            context.MapRoute(
                name: "Facebook/Tin-Nhan",
                url: "Facebook/Tin-Nhan",
                defaults: new { controller = "FAB_LiveChat", action = "FAB_LiveChatView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Facebook.Controllers" });
            #endregion

            #region FAB_COMMENT
            context.MapRoute(
                name: "Facebook/Binh-Luan",
                url: "Facebook/Binh-Luan",
                defaults: new { controller = "FAB_Comment", action = "FAB_CommentView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Facebook.Controllers" });
            #endregion

            #region FAB_PAGECONFIGURATION
            context.MapRoute(
                name: "Facebook/Cau-Hinh-Tuong-Tac-Trang",
                url: "Facebook/Cau-Hinh-Tuong-Tac-Trang",
                defaults: new { controller = "FAB_PageConfiguration", action = "FAB_PageConfigurationView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Facebook.Controllers" });
            #endregion

            #region FAB_PAGECONFIGURATION
            context.MapRoute(
                name: "Facebook/Khach-Hang-Tiem-Nang",
                url: "Facebook/Khach-Hang-Tiem-Nang",
                defaults: new { controller = "FAB_Follower", action = "FAB_FollowerView" },
                namespaces: new string[] { "EMS.MVC.Frontend.Areas.Facebook.Controllers" });
            #endregion

            context.MapRoute(
                "Facebook_default",
                "Facebook/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}