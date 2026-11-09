using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Areas.Facebook.Models;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Facebook.Controllers
{
    public class FAB_HandleController : FAB_BaseController
    {
        // GET: Facebook/FAB_Handle
        public ActionResult FAB_SelectPageView()
        {
            ViewBag.PageID = XHelper.Session.FacebookProfile.PageID;
            ViewBag.ListPageConnect = FacebookRepository.ListPageConnect("Facebook");
            return View();
        }
        public JsonResult SyncData()
        {
            ZResponse res = new ZResponse();
            res.Code = "00";
            res.Message = "Vui lòng chờ trong giây lát, dữ liệu sẽ được cập nhật ngay.";
            FacebookRepository.SyncFeed(); // SYNC FEED
            FacebookRepository.SyncComment(); // SYNC COMMENT
            FacebookRepository.SyncMessage(); // SYNC MESSAGE
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}