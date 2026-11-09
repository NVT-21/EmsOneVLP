using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.MVC.Frontend.Models;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Repositories;

namespace EMS.MVC.Frontend.Controllers
{
    public class EmployeeGroupController : BaseController
    {
        #region Danh sách nhóm nhân viên
        public ActionResult EmployeeGroupView()
        {
            //ViewBag.ListEmployeeGroupCode = XCommon.GetListEmployeeGroupCode();
            //ViewBag.ListEmployeeGroupName = XCommon.GetListEmployeeGroupName("");
            return PartialView();
        }

        public ActionResult SearchListEmployeeGroupView(ZEmployeeGroupListRequest req)
        {
            List<EmployeeGroupListViewModel> oListItem = new List<EmployeeGroupListViewModel>();
            var res = EmployeeGroupRepository.SearchListEmployeeGroup(new ZEmployeeGroupListRequest() 
            {
                Code = req.Code,
                Name = req.Name,
                IsLock = req.IsLock,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZEmployeeGroupListResponse>>(res.Data);
                foreach (var item in list)
                {
                    oListItem.Add(new EmployeeGroupListViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        Description = item.Description,
                        IsLock = item.IsLock,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate,
                    });
                }
            }
            return PartialView(oListItem);
        }
        #endregion

        #region Thêm nhóm nhân viên
        [AuthorizeController(FunctionalTitle = "THEM_NHOM_NHAN_VIEN")]
        public ActionResult AddEmployeeGroupView()
        {
            AddEmployeeGroupViewModel oItem = new AddEmployeeGroupViewModel()
            {
                IsLock = "N",
                ListIsLock = XData.GetListIsLock(),
                ListEmployee = XCommon.SearchListEmployee(new ZEmployeeListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, EmployeeID = XHelper.Session.EmployeeProfile.ID, IsShop = "Y" })
            };
            //ViewBag.ListEmployee = XCommon.GetListEmployeeInPermission();
            return PartialView(oItem);
        }

        public JsonResult JAddEmployeeGroup(AddEmployeeGroupViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = EmployeeGroupRepository.AddEmployeeGroup(new ZEmployeeGroupAddRequest() 
                {
                    Code = req.Code,
                    Name = req.Name,
                    Description = req.Description,
                    IsLock = req.IsLock,
                    strEmployeeID = req.EmployeeID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    CreatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupController.JAddEmployeeGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa nhóm nhân viên
        [AuthorizeController(FunctionalTitle = "SUA_NHOM_NHAN_VIEN")]
        public ActionResult EditEmployeeGroupView(ZEmployeeGroupGetByIDRequest req)
        {
            EditEmployeeGroupViewModel oItem = new EditEmployeeGroupViewModel();
            var res = EmployeeGroupRepository.SearchEmployeeGroupByEmployeeGroupID(new ZEmployeeGroupGetByIDRequest() 
            {
                ID = req.ID,
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZEmployeeGroupGetByIDResponse>(res.Data);
                oItem = new EditEmployeeGroupViewModel()
                {
                    ID = item.ID,
                    Code = item.Code,
                    Name = item.Name,
                    Description = item.Description,
                    IsLock = item.IsLock,
                    ListIsLock = XData.GetListIsLock(),
                    EmployeeID = item.strEmployeeID,
                    ListEmployee = XCommon.SearchListEmployee(new ZEmployeeListRequest() { ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID, EmployeeID = XHelper.Session.EmployeeProfile.ID, IsShop = "Y" })
                };
            }
            //ViewBag.ListEmployee = XCommon.GetListEmployeeInPermission();
            return PartialView(oItem);
        }

        public JsonResult JEditEmployeeGroup(EditEmployeeGroupViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = EmployeeGroupRepository.EditEmployeeGroup(new ZEmployeeGroupEditRequest() 
                {
                    ID = req.ID,
                    Name = req.Name,
                    Description = req.Description,
                    IsLock = req.IsLock,
                    strEmployeeID = req.EmployeeID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupController.JEditEmployeeGroup: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mở/ khóa nhóm nhân viên
        [AuthorizeController(FunctionalTitle = "MO_HOAC_KHOA_NHOM_NHAN_VIEN")]
        public JsonResult JChangeIsLock(ZEmployeeGroupChangeIsLockRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = EmployeeGroupRepository.ChangeIsLock(new ZEmployeeGroupChangeIsLockRequest() 
                {
                    ID = req.ID,
                    IsLock = req.IsLock,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "EmployeeGroupController.JChangeIsLock: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}