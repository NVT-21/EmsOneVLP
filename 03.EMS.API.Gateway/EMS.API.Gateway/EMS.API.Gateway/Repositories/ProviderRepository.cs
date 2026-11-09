using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ProviderRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProviderSearchListRequest>(data);
            return XHelper.Invoke("POST", "Provider/List", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProviderAddRequest>(data);
            return XHelper.Invoke("POST", "Provider/Add", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProviderGetByIDRequest>(data);
            return XHelper.Invoke("GET", "Provider/GetByID?ID=" + req.ID + "&ShopID=" + req.ShopID, null);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProviderEditRequest>(data);
            return XHelper.Invoke("POST", "Provider/Edit", req);
        }
        public static ZResponse ChangeIsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProviderChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "Provider/ChangeIsLock", req);
        }
    }
}