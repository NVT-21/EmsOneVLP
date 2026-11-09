using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Repositories
{
    public class ConfigurationRepositories
    {
        public static ZResponse UpdateAutoRequestOrder(ZShopUpdateAutoRequestOrderRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.Invoke(new ZRequest()
                {
                    Code = "SHOP_UPDATE_AUTO_REQUEST_ORDER",
                    Data = JsonConvert.SerializeObject(req)
                });
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ConfigurationRepositories.SearchListCustomerGroup: " + ex.Message);
            }
            return res;
        }
    }
}