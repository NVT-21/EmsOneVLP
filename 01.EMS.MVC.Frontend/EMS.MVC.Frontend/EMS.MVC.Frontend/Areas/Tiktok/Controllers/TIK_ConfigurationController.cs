using EMS.MVC.Frontend.Areas.Tiktok.Models;
using EMS.MVC.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Tiktok.Controllers
{
    public class TIK_ConfigurationController : Controller
    {
        // GET: Tiktok/TIK_Configuration
        public ActionResult TIK_ConfigurationView()
        {
            return View();
        }
        public ActionResult TIK_ConfigProductView()
        {
            var item = TiktokRepository.ConfigProductByPageID();
            TIK_ConfigProductViewModel model = new TIK_ConfigProductViewModel()
            {
                ID = item.ID,
                PageID = item.PageID,
                IsSyncProduct = item.IsSyncProduct,
                IsSyncProductPrice = item.IsSyncProductPrice,
                IsSyncProductStock = item.IsSyncProductStock
            };
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult TIK_JConfigProductChange(TIK_ConfigProductChangeViewModel req)
        {
            var res = TiktokRepository.ConfigProductChange(req);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}