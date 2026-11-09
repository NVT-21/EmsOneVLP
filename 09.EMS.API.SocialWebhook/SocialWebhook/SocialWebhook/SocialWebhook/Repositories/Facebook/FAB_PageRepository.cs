using EMS.Library;
using Newtonsoft.Json;
using SocialWebhook.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Repositories.Facebook
{
    public class FAB_PageRepository
    {
        public static void SendMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/AddMessageFacebook", data);
            if (res.Code == "00")
            {
                FacebookHub.PageSendMessageToUser(JsonConvert.SerializeObject(data));
                //XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("FAB_UserRepository.SendMessage: api={0}||req={1}||res={2}", "Message/AddMessageFacebook", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("FAB_UserRepository.SendMessage: api={0}||req={1}||res={2}", "Message/AddMessageFacebook", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
    }
}