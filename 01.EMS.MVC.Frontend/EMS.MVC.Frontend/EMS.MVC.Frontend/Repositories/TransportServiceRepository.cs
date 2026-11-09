using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.MVC.Frontend.Repositories
{
    public class TransportServiceRepository
    {
        public static ZResponse SearchListTransportServiceByShopID(ZTransportServiceSearchListByShopIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_TRANSPORT_SERVICE_BY_SHOP_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.SearchListTransportService: " + ex.Message);
            }
            return res;
        }
        //public static ZResponse CheckTransportService(ZTransportServiceCheckRequest req)
        //{
        //    ZResponse res = new ZResponse();
        //    try
        //    {
        //        res = XHelper.Invoke(new ZRequest()
        //        {
        //            Code = "TRANSPORT_SERVICE_CHECK",
        //            Data = JsonConvert.SerializeObject(req)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Code = "99";
        //        res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
        //        XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.CheckTransportService: " + ex.Message);
        //    }
        //    return res;
        //}
        public static ZResponse GetByID(ZTransportServiceGetByIDRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_SERVICE_GET_BY_ID",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.GetByID: " + ex.Message);
            }
            return res;
        }
        public static ZResponse ShopTransportServiceEdit(ZShopTransportServiceEditRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_TRANSPORT_SERVICE_EDIT",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportServiceRepository.ShopTransportServiceEdit: " + ex.Message);
            }
            return res;
        }
    }
}