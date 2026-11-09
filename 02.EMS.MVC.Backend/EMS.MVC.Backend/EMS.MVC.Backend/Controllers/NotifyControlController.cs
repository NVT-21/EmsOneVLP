using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Controllers
{
    public class NotifyControlController : BaseController
    {
        // GET: NotifyControl
        public ActionResult NotifyView()
        {
            NotifyControlSearchListModel model = new NotifyControlSearchListModel()
            {
                FromDate = DateTime.Now.AddDays(-6).ToString("dd/MM/yyyy"),
                ToDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            return View(model);
        }
        public ActionResult SearchListNotifyView(NotifyControlSearchListModel req)
        {
            List<NotifyControlSearchListViewModel> model = new List<NotifyControlSearchListViewModel>();
            var res = NotifyControlRepository.NotifyList(new NotifyControlSearchListModel() {
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                IsLock = req.IsLock
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZNotifyControlListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new NotifyControlSearchListViewModel()
                    {
                        NotifyID = item.NotifyID,
                        NotifyType = item.NotifyType,
                        NotifyTypeName = item.NotifyTypeName,
                        NotifyName = item.NotifyName,
                        NotifyContent = item.NotifyContent,
                        NotifyRedirectUrl = item.NotifyRedirectUrl,
                        NotifyReceiverType = item.NotifyReceiverType,
                        NotifyReceiverTypeName = item.NotifyReceiverTypeName,
                        NotifyReceiverCRM = item.NotifyReceiverCRM,
                        NotifyReceiverGroupName = item.NotifyReceiverGroupName,
                        NotifySendDate = item.NotifySendDate,
                        IsLock = item.IsLock,
                        IsSend = item.IsSend,
                        CreatedByName = item.CreatedByName,
                        CreatedDate = item.CreatedDate,
                        UpdatedDate = item.UpdatedDate,
                        NotifyStatisticalID = item.NotifyStatisticalID,
                        NotifyQuantitySend = item.NotifyQuantitySend,
                        NotifyQuantityViewed = item.NotifyQuantityViewed,
                        NotifyQuantityUnViewed = item.NotifyQuantitySend - item.NotifyQuantityViewed
                    });
                }
            }
            return PartialView(model);
        }
        [AuthorizeController]
        public ActionResult NotifyAddView()
        {
            NotifyControlAddModel model = new NotifyControlAddModel()
            {
                NotifySendDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                ListNotifyReceiverGroupID = CustomerGroupRepository.ListItemCustomerGroup()
            };
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult JAdd(NotifyControlAddModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = NotifyControlRepository.NotifyAdd(new ZNotifyControlAddRequest() {
                    NotifyType = "EMS",
                    NotifyName = req.NotifyName,
                    NotifyContent = req.NotifyContent,
                    NotifyRedirectUrl = req.NotifyRedirectUrl,
                    NotifyReceiverType = req.NotifyReceiverType,
                    NotifyReceiverGroupID = req.NotifyReceiverGroupID,
                    NotifyReceiverCRM = req.NotifyReceiverCRM,
                    NotifySendDate = req.NotifySendDate,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControl.JAdd: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult JChangeLock(NotifyControlChangeLockModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = NotifyControlRepository.NotifyChangeLock(new NotifyControlChangeLockModel() {
                    NotifyID = req.NotifyID,
                    IsLock = req.IsLock,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControl.JChangeLock: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult JDelete(NotifyControlRemoveModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = NotifyControlRepository.NotifyRemove(new NotifyControlRemoveModel() { NotifyID = req.NotifyID, UpdatedBy = XHelper.Session.UserProfile.ID });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControl.JDelete: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotifyDetailView(NotifyControlGetByIDModel req)
        {
            NotifyControlSearchListViewModel model = null;
            var res = NotifyControlRepository.NotifyGetByID(new NotifyControlGetByIDModel() { NotifyID = req.NotifyID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZNotifyControlListResponse>(res.Data);
                model = new NotifyControlSearchListViewModel()
                {
                    NotifyID = item.NotifyID,
                    NotifyType = item.NotifyType,
                    NotifyTypeName = item.NotifyTypeName,
                    NotifyName = item.NotifyName,
                    NotifyContent = item.NotifyContent,
                    NotifyRedirectUrl = item.NotifyRedirectUrl,
                    NotifyReceiverType = item.NotifyReceiverType,
                    NotifyReceiverTypeName = item.NotifyReceiverTypeName,
                    NotifyReceiverCRM = item.NotifyReceiverCRM,
                    NotifySendDate = item.NotifySendDate,
                    IsLock = item.IsLock,
                    IsSend = item.IsSend,
                    CreatedDate = item.CreatedDate,
                    UpdatedDate = item.UpdatedDate
                };
            }
            return PartialView(model);
        }
        [AuthorizeController]
        public ActionResult NotifyEditView(NotifyControlGetByIDModel req)
        {
            NotifyControlSearchListViewModel model = null;
            var res = NotifyControlRepository.NotifyGetByID(new NotifyControlGetByIDModel() { NotifyID = req.NotifyID });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZNotifyControlListResponse>(res.Data);
                model = new NotifyControlSearchListViewModel()
                {
                    NotifyID = item.NotifyID,
                    NotifyType = item.NotifyType,
                    NotifyTypeName = item.NotifyTypeName,
                    NotifyName = item.NotifyName,
                    NotifyContent = item.NotifyContent,
                    NotifyRedirectUrl = item.NotifyRedirectUrl,
                    NotifyReceiverType = item.NotifyReceiverType,
                    NotifyReceiverTypeName = item.NotifyReceiverTypeName,
                    NotifyReceiverGroupID = item.NotifyReceiverGroupID,
                    NotifyReceiverCRM = item.NotifyReceiverCRM,
                    NotifySendDate = item.NotifySendDate
                };
            }
            ViewBag.ListNotifyReceiverGroupID = CustomerGroupRepository.ListItemCustomerGroup();
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult JEdit(NotifyControlEditModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = NotifyControlRepository.NotifyEdit(new ZNotifyControlEditRequest()
                {
                    NotifyID = req.NotifyID,
                    NotifyType = req.NotifyType,
                    NotifyName = req.NotifyName,
                    NotifyContent = req.NotifyContent,
                    NotifyRedirectUrl = req.NotifyRedirectUrl,
                    NotifyReceiverType = req.NotifyReceiverType,
                    NotifyReceiverGroupID = req.NotifyReceiverGroupID,
                    NotifyReceiverCRM = req.NotifyReceiverCRM,
                    NotifySendDate = req.NotifySendDate,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "NotifyControl.JEdit: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}