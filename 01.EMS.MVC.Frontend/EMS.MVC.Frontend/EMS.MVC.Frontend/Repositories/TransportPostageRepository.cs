using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;


namespace EMS.MVC.Frontend.Repositories
{
    public class TransportPostageRepository
    {
        public static ZResponse TransportPostageCharged(ZTransportPostageChargedRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "TRANSPORT_POSTAGE_CHARGED",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "TransportPostageRepository.TransportPostageCharged: " + ex.Message);
            }
            return res;
        }
    }
}