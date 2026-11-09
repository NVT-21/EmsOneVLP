using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ZaloRepository
    {
        public static ZResponse Connect(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloConnectRequest>(data);
            return XHelper.Invoke("POST", "Zalo/Connect", req);
        }
        public static ZResponse SearchListOA(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloSearchListOARequest>(data);
            return XHelper.Invoke("POST", "Zalo/SearchListOA", req);
        }
        //public static ZResponse Info(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZZaloRequest>(data);
        //    return XHelper.Invoke("POST", "Zalo/Info", req);
        //}
        //public static ZResponse UpdateToken(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZZaloRequest>(data);
        //    return XHelper.Invoke("POST", "Zalo/UpdateToken", req);
        //}
        //public static ZResponse UpdateInfoOA(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZZaloUpdateInfoOARequest>(data);
        //    return XHelper.Invoke("POST", "Zalo/UpdateInfoOA", req);
        //}
        public static ZResponse GetByOAID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZZaloGetByOfficialAccountIDRequest>(data);
            return XHelper.Invoke("GET", "Zalo/GetByOAID?OfficialAccountID=" + req.OfficialAccountID + "&ShopID=" + req.ShopID, null);
        }
    }
}