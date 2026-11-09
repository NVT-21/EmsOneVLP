using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;
namespace EMS.API.Gateway.Repositories
{
    public class ZaloMessageRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloMessageSearchListRequest>(data);
            return XHelper.Invoke("POST", "ZaloMessage/SearchList", req);
        }
        public static ZResponse UserSendMessageToOA(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloUserSendMessageToOARequest>(data);
            return XHelper.Invoke("POST", "ZaloMessage/UserSendMessageToOA", req);
        }
        public static ZResponse UserReceivedMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloUserReceivedMessageRequest>(data);
            return XHelper.Invoke("POST", "ZaloMessage/UserReceivedMessage", req);
        }
        public static ZResponse UserSeenMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloUserSeenMessageRequest>(data);
            return XHelper.Invoke("POST", "ZaloMessage/UserSeenMessage", req);
        }
        
        public static ZResponse OASendTextToUser(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloOASendTextToUserRequest>(data);
            return XHelper.Invoke("POST", "ZaloMessage/OASendTextToUser", req);
        }
        
    }
}