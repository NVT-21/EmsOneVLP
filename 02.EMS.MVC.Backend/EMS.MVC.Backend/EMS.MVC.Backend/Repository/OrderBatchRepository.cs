using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using EMS.Entity;
using EMS.Library;
namespace EMS.MVC.Backend.Repository
{
    public class OrderBatchRepository
    {
        public static ZResponse DetailByBatchCode(ZOrderBatchDetailByBatchCodeRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "ORDER_BATCH_DETAIL_BY_BATCH_CODE",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "OrderBatchRepository.DetailByBatchCode: " + ex.Message);
            }
            return res;
        }
    }
}