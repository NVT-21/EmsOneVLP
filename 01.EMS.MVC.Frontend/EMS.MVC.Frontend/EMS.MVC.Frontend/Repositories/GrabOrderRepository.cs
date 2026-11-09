using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class GrabOrderRepository
    {
        public static ZResponse GrabDeliveryAddOrder(ZOrderAddRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "GRAB_DELIVERY_ADD_ORDER",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderRepository.GrabDeliveryAddOrder: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GrabDeliveryCancelOrder(ZOrderCancelStatusRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "GRAB_DELIVERY_CANCEL_ORDER",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderRepository.GrabDeliveryCancelOrder: " + ex.Message);
            }
            return res;
        }
        public static ZResponse GrabDeliveryListOrder(ZGrabOrderSearchListRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "GRAB_DELIVERY_ORDER_SEARCH",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "GrabOrderRepository.GrabDeliveryListOrder: " + ex.Message);
            }
            return res;
        }
    }
}