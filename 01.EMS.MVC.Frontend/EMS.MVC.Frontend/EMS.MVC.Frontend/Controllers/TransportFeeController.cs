using EMS.Entity;
using EMS.Library;
using EMS.MVC.Frontend.Models;
using EMS.MVC.Frontend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Controllers
{
    public class TransportFeeController : BaseController
    {
        [AuthorizeController(FunctionalTitle = "UOC_TINH_CUOC_PHI")]
        public ActionResult TransportFeeView()
        {
            TransportFeeViewModel model = new TransportFeeViewModel()
            {
                Weight = 100,
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>(),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest { Type = 1 }),
                ListWard = new List<WardModel>()
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListByShopIDRequest() { ParentID = 0, Channel = "EMSONE", IsLock = "N", ShopID = XHelper.Session.EmployeeProfile.ShopInformation.ID });
            if(ListTransportService.Count > 0)
            {
                model.ListMainTransportService = ListTransportService.Where(m => m.ParentID == 1).ToList();
                model.ListExtraTransportService = ListTransportService.Where(m => m.ParentID == 2).ToList();
            }
            return View(model);
        }

        public JsonResult JTransportFee(TransportFeeViewModel req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = TransportFeeRepsitory.SearchTransportFee(new ZTransportPostageChargedRequest()
                {
                    MainServiceID = req.MainTransportServiceID,
                    ExtraServiceIDs = req.ExtraTransportServiceID,
                    FromProvinceID = req.FromProvinceID,
                    ToProvinceID = req.ToProvinceID,
                    ToWardID = req.ToWardID,
                    Weight = req.Weight,
                    TransportSizeLength = req.Length,
                    TransportSizeWidth = req.Width,
                    TransportSizeHeight = req.Height,
                    CustomerCode = XHelper.Session.EmployeeProfile.ShopInformation.CrmOrPayPostCode,
                    Quantity = req.Quantity,
                    Amount = req.OrderValue,
                    COD = req.TotalCOD
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportFeeController.JTransportFee: " + ex.Message);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}