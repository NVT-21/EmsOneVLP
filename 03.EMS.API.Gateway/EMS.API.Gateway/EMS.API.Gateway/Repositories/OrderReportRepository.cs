using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class OrderReportRepository
    {
        public static ZResponse ListOrderReport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderReportRequest>(data);
            if(XHelper.IsSqlInjection(req.SearchKey) || XHelper.IsSqlInjection(req.PlaceOfCreation))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "OrderReport/ListOrderReport", req);
        }
        public static ZResponse ListOrderDetailReport(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderDetailReportRequest>(data);
            if(XHelper.IsSqlInjection(req.SearchKey) || XHelper.IsSqlInjection(req.PlaceOfCreation))
            {
                return new ZResponse { Code = "99", Message = "Chứa ký tự không hợp lệ" };
            }
            return XHelper.Invoke("POST", "OrderReport/ListOrderDetailReport", req);
        }
    }
}