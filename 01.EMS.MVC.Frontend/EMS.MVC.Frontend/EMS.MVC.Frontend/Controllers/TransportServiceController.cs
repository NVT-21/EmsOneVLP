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
    public class TransportServiceController : BaseController
    {
        #region Danh sách dịch vụ vận chuyển
        public ActionResult TransportServiceView()
        {
            TransportServiceViewModel model = new TransportServiceViewModel() 
            {
                Type = 1
            };
            return View(model);
        }

        public ActionResult SearchListTransportServiceView(TransportServiceViewModel req)
        {
            SearchListTransportServiceViewModel model = new SearchListTransportServiceViewModel() 
            {
                Type = req.Type,
                ListTransportService = new List<TransportServiceModel>(),
            };
            var res = TransportServiceRepository.SearchListTransportServiceByShopID(new ZTransportServiceSearchListByShopIDRequest()
            {
                ParentID = req.Type,
                Channel = "EMSONE",
                IsLock = "",
                ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID
            });
            if (res.Code == "00")
            {
                var ListTransportService = JsonConvert.DeserializeObject<List<ZTransportServiceSearchListByShopIDResponse>>(res.Data);
                foreach(var item in ListTransportService)
                {
                    model.ListTransportService.Add(new TransportServiceModel() 
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                        IsLock = item.IsLock,
                        IsDefault = item.IsDefault
                    });
                }
            }
            return PartialView(model);
        }
        #endregion

        #region Cập nhật dịch vụ vận chuyển
        [AuthorizeController(FunctionalTitle = "CAP_NHAT_DICH_VU_VAN_CHUYEN")]
        public JsonResult JUpdateTransportService(SearchListTransportServiceViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportServiceRepository.ShopTransportServiceEdit(new ZShopTransportServiceEditRequest()
                {
                    Data = req.ServiceID,
                    Type = req.Type,
                    UpdatedBy = XHelper.Session.EmployeeProfile.ID,
                    ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID,
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceController.JUpdateTransportService: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}