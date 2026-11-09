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
    public class ZAL_UserRepository
    {
        public static void SendMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/AddMessageZalo", data);
            if (res.Code == "00")
            {
                ZaloHub.UserSendMessageToPage(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_UserRepository.SendMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void ReceivedMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/RecievedMessageZalo", data);
            if (res.Code == "00")
            {
                ZaloHub.UserReceivedMessage(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_UserRepository.ReceivedMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void SeenMessage(object data)
        {
            var res = XHelpers.Invoke("POST", "Message/SeenMessageZalo", data);
            if (res.Code == "00")
            {
                ZaloHub.UserSeenMessage(JsonConvert.SerializeObject(data));
            }
            else
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_UserRepository.SeenMessage: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void Follow(object data)
        {
            var res = XHelpers.Invoke("POST", "UserChannel/ZaloFollow", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_UserRepository.Follow: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void UnFollow(object data)
        {
            var res = XHelpers.Invoke("POST", "UserChannel/ZaloUnFollow", data);
            if (res.Code != "00")
            {
                XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_UserRepository.UnFollow: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
            }
        }
        public static void SubmitInfo(object data)
        {
            var res = new ZResponse()
            {
                Code = "01",
                Message = "Không tìm thấy mã hàm ở core"
            };
            XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_UserRepository.SubmitInfo: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));

        }
        public static void Authentication(object data)
        {
            var res = new ZResponse()
            {
                Code = "01",
                Message = "Không tìm thấy mã hàm ở core"
            };
            XHelpers.WriteLog(LogFileType.MESSAGE, string.Format("ZAL_UserRepository.Authentication: req={0}||res={1}", JsonConvert.SerializeObject(data), JsonConvert.SerializeObject(res)));
        }
    }
}