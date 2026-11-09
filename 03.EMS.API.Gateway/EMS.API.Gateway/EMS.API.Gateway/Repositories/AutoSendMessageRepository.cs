using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class AutoSendMessageRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZAutoSendMessageSearchListRequest>(data);
            return XHelper.Invoke("POST", "AutoSendMessage/SearchList", req);
        }
        public static ZResponse AddAutoSendMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZAutoSendMessageAddRequest>(data);
            return XHelper.Invoke("POST", "AutoSendMessage/AddAutoSendMessage", req);
        }
    }
}