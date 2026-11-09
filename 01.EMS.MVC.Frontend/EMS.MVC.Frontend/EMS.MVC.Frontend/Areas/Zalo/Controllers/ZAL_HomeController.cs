using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Areas.Zalo.Models;
namespace EMS.MVC.Frontend.Areas.Zalo.Controllers
{
    public class ZAL_HomeController : ZAL_BaseController
    {
        public ActionResult ZAL_DashboardView()
        {
            ZAL_DashboardViewModel model = new ZAL_DashboardViewModel() 
            {
                PageID = XHelper.Session.ZaloProfile.PageID,
                PageName = XHelper.Session.ZaloProfile.PageName,
                PageAvatar = XHelper.Session.ZaloProfile.PageAvatar,
                PageCover = XHelper.Session.ZaloProfile.PageCover,
                PageDescription = XHelper.Session.ZaloProfile.PageDescription,
                AccessToken = XHelper.Session.ZaloProfile.AccessToken,
                RefreshToken = XHelper.Session.ZaloProfile.RefreshToken
            };
            return View(model);
        }
    }
}