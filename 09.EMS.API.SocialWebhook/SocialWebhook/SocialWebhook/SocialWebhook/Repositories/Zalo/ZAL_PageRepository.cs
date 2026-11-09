using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using SocialWebhook.Hubs;

namespace SocialWebhook.Repositories.Zalo
{
    public class ZAL_PageRepository
    {
        public static void SendMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/AddMessageZalo", data);
            if (res.Code == "00")
            {
                ZaloHub.PageSendMessageToUser(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_PageRepository.SendMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
    }
}