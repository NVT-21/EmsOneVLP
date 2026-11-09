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
    public class TIK_ReceiveStatusRepository
    {
        public static void ReceiveStatus(TiktokReceiveStatusRequest request)
        {
            ZTikReceiveStatusRequest dataRequest = new ZTikReceiveStatusRequest()
            {
                OrderCode = request.Data.OrderID,
                StatusCode = request.Data.OrderStatus,
                DateTime = XData.TimeStampSecondToDateTimeString(request.Data.UpdateTime)
            };
            var res = XHelpers.Invoke("POST", "TiktokWebhook/ReceiveStatus", dataRequest);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("TIK_ReceiveStatusRepository.ReceiveStatus: api={0}||req={1}||res={2}", "TiktokWebhook/ReceiveStatus", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(res)));
            }
        }
    }
}