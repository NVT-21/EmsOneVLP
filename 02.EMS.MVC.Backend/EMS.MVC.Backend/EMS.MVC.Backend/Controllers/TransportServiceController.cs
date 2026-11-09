using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;

namespace EMS.MVC.Backend.Controllers
{
    public class TransportServiceController : BaseController
    {
        #region Danh sách dịch vụ vận chuyển
        public ActionResult TransportServiceView()
        {
            TransportServiceViewModel model = new TransportServiceViewModel()
            {
                ParentID = 1,
            };
            return View(model);
        }

        public ActionResult SearchListTransportServiceView(TransportServiceViewModel req)
        {
            List<SearchListTransportServiceViewModel> model = new List<SearchListTransportServiceViewModel>();
            var res = TransportServiceRepository.SearchList(new ZTransportServiceSearchListRequest()
            {
                ParentID = req.ParentID,
                Channel = "EMSONE"
            });
            if (res.Code == "00")
            {
                var list = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListResponse>>(res.Data);
                foreach (var item in list)
                {
                    model.Add(new SearchListTransportServiceViewModel()
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        ParentID = item.ParentID,
                        SpecialService = new BaseModel()
                        {
                            Code = item.SpecialService.Code,
                            Name = item.SpecialService.Name
                        },
                        IsLock = item.IsLock,
                        CreatedBy = item.CreatedBy,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            ViewBag.ParentID = req.ParentID;
            return PartialView(model);
        }
        #endregion

        #region Thêm dịch vụ vận chuyển
        public ActionResult AddTransportServiceView(SearchListTransportServiceViewModel req)
        {
            AddTransportServiceViewModel model = new AddTransportServiceViewModel()
            {
                ParentID = req.ParentID,
                ListTransportServiceClassification = XData.GetListTransportServiceClassification(),
                ListTransportServiceType = XData.GetListTransportServiceType()
            };
            return PartialView(model);
        }

        public JsonResult JAddTransportService(AddTransportServiceViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportServiceRepository.AddTransportService(new ZTransportServiceAddRequest()
                {
                    ParentID = req.ParentID,
                    Code = req.Code,
                    Name = req.Name,
                    Classification = req.Classification,
                    Type = req.Type,
                    IsSpecial = req.IsSpecial == true ? "Y" : "N",
                    Description = req.Description,
                    CreatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceController.JAddTransportService: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sửa dịch vụ vận chuyển
        public ActionResult EditTransportServiceView(SearchListTransportServiceViewModel req)
        {
            EditTransportServiceViewModel model = null;
            var res = TransportServiceRepository.GetByID(new ZTransportServiceGetByIDRequest()
            {
                ID = req.ID
            });
            if (res.Code == "00")
            {
                var item = JsonConvert.DeserializeObject<ZTransportServiceGetByIDResponse>(res.Data);
                model = new EditTransportServiceViewModel()
                {
                    ID = item.ID,
                    ParentID = item.ParentID,
                    Code = item.Code,
                    Name = item.Name,
                    Classification = item.Classification,
                    ListTransportServiceClassification = XData.GetListTransportServiceClassification(),
                    Type = item.Type,
                    ListTransportServiceType = XData.GetListTransportServiceType(),
                    IsSpecial = item.IsSpecial == "Y" ? true : false,
                    Description = item.Description
                };
            }
            return PartialView(model);
        }

        public JsonResult JEditTransportService(EditTransportServiceViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportServiceRepository.EditTransportService(new ZTransportServiceEditRequest() 
                {
                    ID = req.ID,
                    ParentID = req.ParentID,
                    Name = req.Name,
                    Classification = req.Classification,
                    Type = req.Type,
                    IsSpecial = req.IsSpecial == true ? "Y" : "N",
                    Description = req.Description,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceController.JEditTransportService: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Khóa dịch vụ vận chuyển
        public JsonResult JLock(SearchListTransportServiceViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportServiceRepository.LockTransportService(new ZTransportServiceLockRequest()
                {
                    ID = req.ID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceController.JLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Mở khóa dịch vụ vận chuyển
        public JsonResult JUnLock(SearchListTransportServiceViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportServiceRepository.UnLockTransportService(new ZTransportServiceUnLockRequest() 
                {
                    ID = req.ID,
                    UpdatedBy = XHelper.Session.UserProfile.ID
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceController.JUnLock: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}