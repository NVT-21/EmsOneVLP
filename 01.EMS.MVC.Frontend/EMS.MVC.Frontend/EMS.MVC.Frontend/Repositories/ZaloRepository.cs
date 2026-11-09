using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
namespace EMS.MVC.Frontend.Repositories
{
    public class ZaloRepository
    {
        public static ZResponse SendMessage(ZZaloSendMessageRequest req)
        {
            ZResponse res = new ZResponse();
            try
            {
                res = XHelper.InvokeZaloV1(new ZRequest()
                {
                    Code = "https://openapi.zalo.me/v2.0/oa/message",
                    Data = JsonConvert.SerializeObject(req)
                }, "POST", req.AccessToken);
            }
            catch (Exception ex)
            {
                res.Code = "99";
                res.Message = "Lỗi hệ thống, vui lòng liên hệ quản trị.";
                XHelper.LogToFile(LogFileType.EXCEPTION, "ZaloRepository.SendMessage: " + ex.Message);
            }
            return res;
        }
    }
}