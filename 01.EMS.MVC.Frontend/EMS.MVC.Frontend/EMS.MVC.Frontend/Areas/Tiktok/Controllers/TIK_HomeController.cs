using EMS.MVC.Frontend.Areas.Tiktok.Models;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Tiktok.Controllers
{
    public class TIK_HomeController : TIK_BaseController
    {
        // GET: Tiktok/TIK_Home
        public ActionResult TIK_DashboardView()
        {
            TIK_DashboardViewModel model = new TIK_DashboardViewModel()
            {
                PageID = XHelper.Session.TiktokProfile.PageID,
                PageName = XHelper.Session.TiktokProfile.PageName,
                PageAvatar = XHelper.Session.TiktokProfile.PageAvatar,
                PageCategory = XHelper.Session.TiktokProfile.PageCategory,
                PageAccessToken = XHelper.Session.TiktokProfile.PageAccessToken,
                UserAccessToken = XHelper.Session.TiktokProfile.UserAccessToken
            };
            return View(model);
        }
    }
}