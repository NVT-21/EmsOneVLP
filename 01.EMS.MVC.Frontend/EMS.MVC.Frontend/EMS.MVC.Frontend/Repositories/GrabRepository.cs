using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Repositories
{
    public class GrabRepository
    {
        public static ZResponse GrabListService()
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "GRAB_DELIVERY_SERVICE",
                    Data = ""
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabRepository.GrabListService: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GrabDeliveryQuotation(ZGrabTransportFeeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "GRAB_DELIVERY_QUOTATION",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabRepository.GrabDeliveryQuotation: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GrabDeliveryDetail(ZGrabDetailByShippingIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeSyncGateway(new ZRequest()
                {
                    Code = "GRAB_DELIVERY_DETAIL",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabRepository.GrabDeliveryDetail: " + ex.Message);
            }
            return res;
        }
    }
}