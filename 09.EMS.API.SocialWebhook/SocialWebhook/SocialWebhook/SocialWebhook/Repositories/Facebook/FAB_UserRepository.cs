using EMS.Library;
using Newtonsoft.Json;
using SocialWebhook.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Repositories.Facebook
{
    public class FAB_UserRepository
    {
        public static void SendMessage(object data)
        {
            //Gửi tin nhắn
            var res = XHelpers.Invoke("POST", "Message/AddMessageFacebook", data);
            if (res.Code == "00")
            {
                FacebookHub.UserSendMessageToPage(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("FAB_UserRepository.SendMessage: api={0}||req={1}||res={2}", "Message/AddMessageFacebook", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void ReceivedMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/RecievedMessageFacebook", data);
            if (res.Code == "00")
            {
                FacebookHub.UserReceivedMessage(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("FacebookController.Post: api={0}||req={1}||res={2}", "Message/RecievedMessageFacebook", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void SeenMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/SeenMessageFacebook", data);
            if (res.Code == "00")
            {
                FacebookHub.UserSeenMessage(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.EXCEPTION, string.Format("FacebookController.Post: api={0}||req={1}||res={2}", "Message/SeenMessageFacebook", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
    }
}