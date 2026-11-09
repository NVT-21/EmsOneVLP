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
    public class TIK_ReverseStatusRepository
    {
        public static void ReverseStatus(TiktokReverseStatusRequest request)
        {
            ZTikReverseStatusRequest dataRequest = new ZTikReverseStatusRequest()
            {
                OrderCode = request.Data.OrderID,
                ReverseOrderCode = request.Data.ReverseOrderID,
                StatusCode = request.Data.ReverseOrderStatus,
                DateTime = XData.TimeStampSecondToDateTimeString(request.Data.UpdateTime)
            };
            var res = XHelpers.Invoke("POST", "TiktokWebhook/ReverseStatus", dataRequest);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("TIK_ReverseStatusRepository.ReverseStatus: api={0}||req={1}||res={2}", "TiktokWebhook/ReverseStatus", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(res)));
            }
        }
    }
}