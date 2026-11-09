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
    public class EmployeeController : BaseController
    {
        public ActionResult EmployeeView()
        {
            ViewBag.ShopID = XHelper.Encrypt(XHelper.Session.EmployeeProfile.ShopInformation.ID.ToString());
            return PartialView();
        }
        public ActionResult ListEmployeeData(string ShopID)
        {
            List<EmployeeListViewModel> oListItem = new List<EmployeeListViewModel>();
            var res = EmployeeRepository.SearchListEmployee(new ZEmployeeListRequest()
            {
                ShopID = ShopID == "" ? XHelper.Session.EmployeeProfile.ShopInformation.ID : Convert.ToInt32(XHelper.Decrypt(ShopID)),
                EmployeeID = XHelper.Session.EmployeeProfile.ID,
                IsShop = "Y"
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZEmployeeListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new EmployeeListViewModel()
                    {
                        ID = XHelper.Encrypt(item.ID.ToString()),
                        MobileNumber = item.MobileNumber,
                        FullName = item.FullName,
                        IsLock = item.IsLock,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                        CountActiveTime = item.CountActiveTime,
                        GroupPermissionName = item.GroupPermissionName,
                    });
                }
            }
            ViewBag.ShopID = ShopID == "" ? XHelper.Encrypt(XHelper.Session.EmployeeProfile.ShopInformation.ID.ToString()) : ShopID;
            ViewBag.Name = XHelper.Session.EmployeeProfile.ShopInformation.StoreName;
            return PartialView(oListItem);
        }

        [AuthorizeController(FunctionalTitle = "THEM_NHAN_VIEN")]
        public ActionResult AddEmployeeView(int ShopLevel)
        {
            ViewBag.ListGroupPermission = XCommon.GetListGroupPermission(ShopLevel);
            return PartialView();
        }

        public JsonResult AddEmployee(ZEmployeeShopAddViewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_SHOP_ADD",
                    Data = JsonConvert.SerializeObject(new ZEmployeeShopAddRequest()
                    {
                        FullName = req.FullName,
                        MobileNumber = req.MobileNumber,
                        strGroupPermissionID = req.strGroupPermissionID,
                        IsLock = req.IsLock,
                        Description = req.Description,
                        //PassWord = XHelper.PasswordDefault,
                        ShopID = Convert.ToInt32(XHelper.Decrypt(req.ShopID)),
                        CreatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.AddEmployee: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeController(FunctionalTitle = "MO_HOAC_KHOA_NHAN_VIEN")]
        public JsonResult ChangeIsLock(ZEmployeeChangeIsLockViewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_CHANGE_IS_LOCK",
                    Data = JsonConvert.SerializeObject(new ZEmployeeChangeIsLockRequest()
                    {
                        ID = Convert.ToInt32(XHelper.Decrypt(req.ID)),
                        IsLock = req.IsLock,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ChangeIsLock: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeController(FunctionalTitle = "CHI_TIET_NHAN_VIEN")]
        public ActionResult DetailView(ZEmployeeGetByIDViewRequest req)
        {
            EmployeeDetailViewModel oItem = new EmployeeDetailViewModel();
            var res = XHelper.Invoke(new ZRequest()
            {
                Code = "EMPLOYEE_GET_BY_ID",
                Data = JsonConvert.SerializeObject(new ZEmployeeGetByIDRequest()
                {
                    ID = Convert.ToInt32(XHelper.Decrypt(req.ID)),
                    ShopID = Convert.ToInt32(XHelper.Decrypt(req.ShopID))
                })
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZEmployeeGetByIDResponse>(res.Data);
                oItem = new EmployeeDetailViewModel()
                {
                    ID = XHelper.Encrypt(item.ID.ToString()),
                    UserName = item.UserName,
                    FullName = item.FullName,
                    Email = item.Email,
                    MobileNumber = item.MobileNumber,
                    EmployeeLevel = item.EmployeeLevel,
                    IsLock = item.IsLock,
                    CountActiveTime = item.CountActiveTime,
                    Description = item.Description,
                    strGroupPermissionID = item.strGroupPermissionID,
                    ShopID = XHelper.Encrypt(item.ShopID.ToString())
                };
            }
            ViewBag.ListGroupPermission = XCommon.GetListGroupPermission(req.ShopLevel);
            ViewBag.ListIsLock = XData.GetListIsLock();
            return PartialView(oItem);
        }

        [AuthorizeController(FunctionalTitle = "SUA_NHAN_VIEN")]
        public JsonResult EditEmployee(ZEmployeeShopEditViewRequest req)
        {
            ZResponse res = new ZResponse();
            //res = XCommon.CheckPermission("Employee", "EditEmployee");
            //if (res.Code == "00")
            //{
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_SHOP_EDIT",
                    Data = JsonConvert.SerializeObject(new ZEmployeeShopEditRequest()
                    {
                        ID = Convert.ToInt32(XHelper.Decrypt(req.ID)),
                        ShopID = Convert.ToInt32(XHelper.Decrypt(req.ShopID)),
                        FullName = req.FullName,
                        strGroupPermissionID = req.strGroupPermissionID,
                        IsLock = req.IsLock,
                        Description = req.Description,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.EditEmployee: " + ex.Message);
            }
            //}
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeController(FunctionalTitle = "KHOI_PHUC_MAT_KHAU_CUA_NHAN_VIEN")]
        public JsonResult ResetPassword(string ID, string UserName)
        {
            ZResponse res = new ZResponse();
            //res = XCommon.CheckPermission("Employee", "ResetPassword");
            //if (res.Code == "00")
            //{
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_RESET_PASSWORD",
                    Data = JsonConvert.SerializeObject(new ZEmployeeResetPasswordRequest()
                    {
                        ID = Convert.ToInt32(XHelper.Decrypt(ID)),
                        UserName = UserName,
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.ResetPassword: " + ex.Message);
            }
            //}
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeController(FunctionalTitle = "XOA_NHAN_VIEN")]
        public JsonResult JRemoveEmployee(ZEmployeeRemoveViewRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "EMPLOYEE_REMOVE",
                    Data = JsonConvert.SerializeObject(new ZEmployeeRemoveRequest()
                    {
                        ID = Convert.ToInt32(XHelper.Decrypt(req.ID)),
                        ShopID = Convert.ToInt32(XHelper.Decrypt(req.ShopID)),
                        UpdatedBy = XHelper.Session.EmployeeProfile.ID
                    })
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeController.JRemoveEmployee: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}