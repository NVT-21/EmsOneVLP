using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EMS.API.Gateway.Repositories
{
    public class ReconciliationNotifyRepository
    {
        public static ZResponse ListNotify(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationListNotifyRequest>(data);
            return XHelper.Invoke("POST", "ReconciliationNotify/ListNotify", req);
        }
        public static ZResponse UpdateNotify(string data)
        {
            var req = JsonConvert.DeserializeObject<ZReconciliationUpdateNotifyRequest>(data);
            return XHelper.Invoke("POST", "ReconciliationNotify/UpdateNotify", req);
        }
    }
}