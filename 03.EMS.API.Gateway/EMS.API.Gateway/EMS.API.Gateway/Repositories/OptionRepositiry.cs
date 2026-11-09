using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public static class OptionRepositiry
    {
        public static ZResponse Add (string data)
        {
            var req = JsonConvert.DeserializeObject<ZOptionAddRequest>(data);
            return XHelper.Invoke("POST", "Option/Add", req);
        }
        public static ZResponse RemoveByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOptionRemoveByIDRequest>(data);
            return XHelper.Invoke("POST", "Option/RemoveByID", req);
        }
        public static ZResponse GetOptionByShopID (string data)
        {
            var req = JsonConvert.DeserializeObject<ZOptionGetDataByShopID>(data);
            return XHelper.Invoke("POST", "Option/GetOptionByShopID", req);
        }
        public static ZResponse GetDataByParentID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOptionGetDataByParentIDRequest>(data);
            return XHelper.Invoke("POST", "Option/GetDataByParentID", req);
        }
        public static ZResponse DataByVariantID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOptionDataByVariantIDRequest>(data);
            return XHelper.Invoke("POST", "Option/DataByVariantID", req);
        }
    }
}