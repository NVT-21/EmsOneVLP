using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class PeriodRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPeriodAddRequest>(data);
            return XHelper.Invoke("POST", "Period/Add", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPeriodRemoveRequest>(data);
            return XHelper.Invoke("POST", "Period/Remove", req);
        }
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPeriodListRequest>(data);
            return XHelper.Invoke("POST", "Period/List", req);
        }
    }
}