using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class MessageRepository
    {
        public static ZResponse SearchListMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZMessageSearchListRequest>(data);
            return XHelper.Invoke("POST", "Message/SearchList", req);
        }

        public static ZResponse AddSyncMessage(string data)
        {
            var req = JsonConvert.DeserializeObject<List<ZMessageAddSyncRequest>>(data);
            return XHelper.Invoke("POST", "Message/AddSyncMessage", req);
        }
    }
}