using EMS.MVC.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Tiktok.Controllers
{
    public class TIK_HandleController : TIK_BaseController
    {
        // GET: Tiktok/TIK_Handle
        public ActionResult TIK_SelectPageView()
        {
            ViewBag.PageID = XHelper.Session.TiktokProfile.PageID;
            ViewBag.ListPageConnect = TiktokRepository.ListPageConnect("TiktokShop");
            return View();
        }
    }
}