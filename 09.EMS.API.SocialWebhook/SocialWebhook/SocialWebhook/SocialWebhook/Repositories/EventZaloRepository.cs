using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;
using SocialWebhook.Hubs;
namespace SocialWebhook.Repositories
{
    public class EventZaloRepository
    {
        public static void UserSendMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/AddMessageZalo", data);
            if(res.Code == "00")
            {
                ZaloHub.UserSendMessageToPage(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.UserSendMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void PageSendMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/AddMessageZalo", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.PageSendMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void AddMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/AddMessageZalo", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.AddMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
            else
            {
                //ZaloHub.PushMessageToAllUsers(JsonConvert.SerializeObject(data));
            }
        }
        public static void ReceivedMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/RecievedMessageZalo", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.ReceivedMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void SeenMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/SeenMessageZalo", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.SeenMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void Follow(object data)
        {
            var res = XHelpers.Invoke("POST", "UserChannel/ZaloFollow", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.Follow: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void UnFollow(object data)
        {
            var res = XHelpers.Invoke("POST", "UserChannel/ZaloUnFollow", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.UnFollow: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void SubmitInfo(object data)
        {
            var res = new ZResponse()
            {
                Code = "01",
                Message = "Không tìm thấy mã hàm ở core"
            };
            XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.SubmitInfo: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));

        }
        public static void Authentication(object data)
        {
            var res = new ZResponse()
            {
                Code = "01",
                Message = "Không tìm thấy mã hàm ở core"
            };
            XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("EventZaloRepository.Authentication: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
        }
    }
}