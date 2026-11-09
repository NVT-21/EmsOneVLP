using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using SocialWebhook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Repositories.Tiktok
{
    public class TIK_WebhookDataLogRepository
    {
        public static void AddDataLog(TiktokReceiveStatusRequest request)
        {
            var res = XHelpers.Invoke("POST", "TiktokWebhook/AddDataLog", new ZTikWebhookAddDataLogRequest()
            {
                Type = request.Type,
                PageID = request.ShopID,
                Timestamp = XData.TimeStampSecondToDateTimeString(request.Timestamp),
                OrderID = request.Data.OrderID,
                OrderStatus = request.Data.OrderStatus,
                UpdateTime = XData.TimeStampSecondToDateTimeString(request.Data.UpdateTime)
            });
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("TIK_WebhookDataLogRepository.AddDataLog: api={0}||req={1}||res={2}", "TiktokSync/WebhookAddDataLog", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(res)));
            }
        }
    }
}