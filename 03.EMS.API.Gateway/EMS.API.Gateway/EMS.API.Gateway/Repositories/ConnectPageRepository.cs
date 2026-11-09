using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ConnectPageRepository
    {
        public static ZResponse AddConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConnectPageAddRequest>(data);
            return XHelper.Invoke("POST", "ConnectPage/AddConnectPage", req);
        }
        public static ZResponse SearchListConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConnectPageSearchListRequest>(data);
            return XHelper.Invoke("POST", "ConnectPage/SearchListConnectPage", req);
        }
        public static ZResponse ResetConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConnectPageResetRequest>(data);
            return XHelper.Invoke("POST", "ConnectPage/ResetConnectPage", req);
        }
        public static ZResponse CountFollow(string data)
        {
            var req = JsonConvert.DeserializeObject<ZConnectPageCountFollowRequest>(data);
            return XHelper.Invoke("POST", "ConnectPage/CountFollow", req);
        }
        public static ZResponse DeleteConnectPage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFacebookDeleteConnectRequest>(data);
            return XHelper.Invoke("POST", "ConnectPage/DeleteConnectPage", req);
        }
    }
    
}