using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Repositories;
namespace EMS.MVC.Frontend.Controllers
{
    public class PermissionController : BaseController
    {
        public ActionResult PermissionView()
        {
            if (XHelper.Session.EmployeeProfile.StoreOwner == "N")
            {
                return RedirectToAction("NotPermission", "Error");
            }

            PermissionViewModel model = new PermissionViewModel()
            {
                ShopLevel = 1,
                ListShopLevel = XData.GetListShopLevel(),
                ListGroupPermission = XCommon.SearchListGroupPermission(new ZShopGroupPermissionListRequest() { ShopLevel = 1, IsLock = "N", IsShop = "Y" }),
            };
            return View(model);
        }

        public ActionResult SearchListFunctionalView(PermissionViewModel req)
        {
            SearchListFunctionalViewModel model = new SearchListFunctionalViewModel()
            {
                GroupPermissionID = req.GroupPermissionID
            };
            return PartialView(model);
        }

        [AuthorizeController(FunctionalTitle = "CAP_NHAT_PHAN_QUYEN")]
        public JsonResult JChangeFunctional(PermissionViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = PermissionRepository.ChangeFunctional(new ZFunctionalChangeRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    GroupPermissionID = req.GroupPermissionID,
                    strFunctionalID = req.strFunctionalID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "PermissionController.JChangeFunctional: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchListMenuView()
        {
            List<SearchListMenuViewModel> model = new List<SearchListMenuViewModel>();
            if (XHelper.Session.EmployeeProfile.StoreOwner == "N")
            {
                var res = PermissionRepository.SearchListMenu(new ZFunctionalListMenuRequest()
                {
                    EmployeeID = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                });
                if (res.Code == "00")
                {
                    var ListItem = JsonConvert.DeserializeObject<List<ZFunctionalListMenuResponse>>(res.Data);
                    foreach (var Item in ListItem)
                    {
                        model.Add(new SearchListMenuViewModel()
                        {
                            ID = Item.ID,
                            FunctionalCode = Item.FunctionalCode,
                            FunctionalName = Item.FunctionalName,
                            FunctionalLevel = Item.FunctionalLevel,
                            FunctionalParentID = Item.FunctionalParentID,
                            FunctionalTitle = Item.FunctionalTitle,
                            MenuIcon = Item.MenuIcon,
                            IsType = Item.IsType,
                            IsValue = Item.IsValue,
                            ListFunctional = Item.ListFunctional
                        });
                    }
                }
            }
            return PartialView("~/Views/Shared/_MenuLeft.cshtml", model);
        }
    }
}