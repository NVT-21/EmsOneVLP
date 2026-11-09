using System;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;

namespace EMS.MVC.Frontend.Repositories
{
    public class ExpectedDeliveryRepository
    {
        public static ZResponse SearchDate(ZExpectedDeliverySearchDateRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = CommandCodeConst.EXPECTED_DELIVERY_SEARCH_DATE,
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ExpectedDeliveryRepository.SearchDate: " + ex.Message);
            }
            return res;
        }
    }
}