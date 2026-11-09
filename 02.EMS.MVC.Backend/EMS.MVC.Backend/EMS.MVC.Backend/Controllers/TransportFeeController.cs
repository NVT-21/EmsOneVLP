using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using EMS.Entity;
using EMS.Library;
using EMS.MVC.Backend.Models;
using EMS.MVC.Backend.Repository;
using System.Linq;

namespace EMS.MVC.Backend.Controllers
{
    public class TransportFeeController: BaseController
    {
        public ActionResult TransportFeeView()
        {
            TransportFeeViewModel model = new TransportFeeViewModel()
            {
                Weight = 100,
                ListMainTransportService = new List<TransportServiceModel>(),
                ListExtraTransportService = new List<TransportServiceModel>(),
                ListProvince = XCommon.SearchListProvince(new ZProvinceListRequest()),
                ListDistrict = new List<DistrictModel>(),
                ListWard = new List<WardModel>()
            };

            List<TransportServiceModel> ListTransportService = XCommon.SearchListTransportService(new ZTransportServiceSearchListRequest() { ParentID = 0, IsLock = "N", Channel = "EMSONE" });
            if (ListTransportService.Count > 0)
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
                    ToDistrictID = req.ToDistrictID,
                    ToWardID = req.ToWardID,
                    Weight = req.Weight,
                    TransportSizeLength = req.Length,
                    TransportSizeWidth = req.Width,
                    TransportSizeHeight = req.Height,
                    CustomerCode = req.CrmOrPaypostCode,
                    Quantity = req.Quantity,
                    Amount = req.OrderValue,
                    COD = req.TotalCOD
                });
            }
            catch (Exception ex)
            {
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportFeeController.JTransportFee: " + ex.Message);
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}