using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using Newtonsoft.Json;

namespace EMS.MVC.Backend.Controllers
{
    public class ShopGroupPermissionController : BaseController
    {
        // GET: GroupPermission
        [AuthorizeController]
        public ActionResult Index()
        {
            return View();
        }
        [AuthorizeController]
        public ActionResult ListShopGroupPermissionData(ZShopGroupPermissionListRequest req)
        {
            List<ShopGroupPermissionListViewModel> oListItem = new List<ShopGroupPermissionListViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "SHOP_GROUP_PERMISSION_LIST",
                Data = JsonConvert.SerializeObject(new ZShopGroupPermissionListRequest()
                {
                    IsShop = "Y"
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZShopGroupPermissionListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new ShopGroupPermissionListViewModel()
                    {
                        ID = item.ID,
                        GroupPermissionCode = item.GroupPermissionCode,
                        GroupPermissionName = item.GroupPermissionName,
                        ShopLevel = item.ShopLevel,
                        Description = item.Description,
                        IsLock = item.IsLock,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        NumberOfEmployeesInGroup = item.NumberOfEmployeesInGroup
                    });
                }
            }
            return View(oListItem);
        }
        public ActionResult AddShopGroupPermissionView()
        {
            ViewBag.ListShopLevel = XData.GetListShopLevel();
            ViewBag.ListIsLock = XData.GetListIsLock();
            return View();
        }
        public JsonResult AddShopGroupPermission(ZShopGroupPermissionAddRequest req)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GROUP_PERMISSION_ADD",
                    Data = JsonConvert.SerializeObject(new ZShopGroupPermissionAddRequest()
                    {
                        GroupPermissionCode = req.GroupPermissionCode,
                        GroupPermissionName = req.GroupPermissionName,
                        ShopLevel = req.ShopLevel,
                        IsLock = req.IsLock,
                        Description = req.Description,
                        CreatedBy = XHelper.Session.UserProfile.ID
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopGroupPermissionController.AddEmployeeGroupPermission: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }
        public JsonResult ChangeIsLockShopGroupPermission(ZShopGroupPermissionChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GROUP_PERMISSION_CHANGE_IS_LOCK",
                    Data = JsonConvert.SerializeObject(new ZShopGroupPermissionChangeIsLockRequest()
                    {
                        ID = req.ID,
                        IsLock = req.IsLock,
                        UpdatedBy = XHelper.Session.UserProfile.ID,
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopGroupPermissionController.ChangeIsLockShopGroupPermission: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }
        public ActionResult EditShopGroupPermissionView(ZShopGroupPermissionGetByIDRequest req)
        {
            ShopGroupPermissionDetailViewModel oItem = new ShopGroupPermissionDetailViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "SHOP_GROUP_PERMISSION_GET_BY_ID",
                Data = JsonConvert.SerializeObject(new ZShopGroupPermissionGetByIDRequest()
                {
                    ID = req.ID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGroupPermissionGetByIDResponse>(res.Data);
                oItem = new ShopGroupPermissionDetailViewModel()
                {
                    ID = item.ID,
                    GroupPermissionCode = item.GroupPermissionCode,
                    GroupPermissionName = item.GroupPermissionName,
                    ShopLevel = item.ShopLevel,
                    IsLock = item.IsLock,
                    Description = item.Description,
                    NumberOfEmployeesInGroup = req.NumberOfEmployeesInGroup
                };
            }
            ViewBag.ListShopLevel = XData.GetListShopLevel();
            ViewBag.ListIsLock = XData.GetListIsLock();
            return View(oItem);
        }
        public JsonResult EditShopGroupPermission(ZShopGroupPermissionEditRequest req)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_GROUP_PERMISSION_EDIT",
                    Data = JsonConvert.SerializeObject(new ZShopGroupPermissionEditRequest()
                    {
                        ID = req.ID,
                        GroupPermissionName = req.GroupPermissionName,
                        ShopLevel = req.ShopLevel,
                        IsLock = req.IsLock,
                        Description = req.Description,
                        UpdatedBy = XHelper.Session.UserProfile.ID,
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ShopGroupPermissionController.EditShopGroupPermission: " + ex.Message);
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            return jResult;
        }

    }
}