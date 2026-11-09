using System.Collections.Generic;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using EMS.Entity;
using EMS.MVC.Frontend.Repositories;
using EMS.Library;
using System;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Controllers
{
    public class ConfigurationController : BaseController
    {
        #region Tình trạng xử lý
        public ActionResult StatusProcessView()
        {
            return View();
        }

        public ActionResult ListStatusProcessData()
        {
            List<StatusProcessListViewModel> oListItem = new List<StatusProcessListViewModel>();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "STATUS_PROCESS_LIST",
                Data = JsonConvert.SerializeObject(new ZStatusProcessListRequest()
                {
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZStatusProcessListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new StatusProcessListViewModel()
                    {
                        ID = item.ID,
                        Note = item.Note,
                        IsLock = item.IsLock,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return View(oListItem);
        }

        public ActionResult AddStatusProcessView()
        {
            return View();
        }

        public JsonResult JAddStatusProcess(StatusProcessAddModel req)
        {
            ZResponse res = new ZResponse();
            JsonResult jResult = new JsonResult();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STATUS_PROCESS_ADD",
                    Data = JsonConvert.SerializeObject(new ZStatusProcessAddRequest()
                    {
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        Note = req.Note,
                        CreatedBy = XHelper.Session.EmployeeProfile.ID,
                    })
                });
                jResult = Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConfigurationController.JAddStatusProcess: " + ex.Message);
                return jResult;
            }
            return jResult;
        }

        public JsonResult JChangeIsLockStatusProcess(StatusProcessChangeIsLockModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STATUS_PROCESS_CHANGE_IS_LOCK",
                    Data = JsonConvert.SerializeObject(new ZStatusProcessChangeIsLockRequest()
                    {
                        ID = req.ID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        IsLock = req.IsLock,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConfigurationController.JChangeIsLockStatusProcess: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JRemoveStatusProcess(StatusProcessRemoveModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "STATUS_PROCESS_REMOVE",
                    Data = JsonConvert.SerializeObject(new ZStatusProcessRemoveRequest()
                    {
                        ID = req.ID,
                        ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConfigurationController.JRemoveStatusProcess: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Tự động chia yêu cầu đặt hàng 
        [AuthorizeController]
        public ActionResult AutoRequestOrderView()
        {
            AutoRequestOrderViewModel oItem = null;
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "SHOP_GET_BY_ID",
                Data = JsonConvert.SerializeObject(new ZShopGetByIDRequest()
                {
                    ID = XHelper.Session.EmployeeProfile.ShopInformation.ID
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZShopGetByIDResponse>(res.Data);
                oItem = new AutoRequestOrderViewModel()
                {
                    AutoDivision = item.AutoDivision,
                    EmployeeReadyDivision = item.EmployeeReadyDivision,
                    EmployeeGroupPermissionID = item.EmployeeGroupPermissionID,
                    DateProcessRequestOrder = item.DateProcessRequestOrder,
                    Criteria = item.AutoCriteria,
                    TimeDivision = item.AutoTimeDivision,
                    ListGroupPermission = XCommon.SearchListGroupPermission(new ZShopGroupPermissionListRequest() { ShopLevel = 1, IsLock = "N", IsShop = "Y" }),
                };
            }
            return View(oItem);
        }

        public JsonResult AutoRequestOrder(ZShopUpdateAutoRequestOrderRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = ConfigurationRepositories.UpdateAutoRequestOrder(new ZShopUpdateAutoRequestOrderRequest()
                {
                    ID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    AutoDivision = req.AutoDivision,
                    EmployeeReadyDivision = req.EmployeeReadyDivision,
                    EmployeeGroupPermissionID = req.EmployeeGroupPermissionID,
                    DateProcessRequestOrder = req.DateProcessRequestOrder,
                    Criteria = req.Criteria,
                    TimeDivision = req.TimeDivision,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConfigurationController.AutoRequestOrder: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}