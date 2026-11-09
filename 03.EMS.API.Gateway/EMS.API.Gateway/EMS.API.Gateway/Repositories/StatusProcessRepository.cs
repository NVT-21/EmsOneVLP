using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class StatusProcessRepository
    {
        public static ZResponse ListStatusProcess(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatusProcessListRequest>(data);
            return XHelper.Invoke("POST", "StatusProcess/List", req);
        }
        public static ZResponse AddStatusProcess(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatusProcessAddRequest>(data);
            return XHelper.Invoke("POST", "StatusProcess/Add", req);
        }
        public static ZResponse ChangeIsLockStatusProcess(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatusProcessChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "StatusProcess/ChangeIsLock", req);
        }
        public static ZResponse RemoveStatusProcess(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStatusProcessRemoveRequest>(data);
            return XHelper.Invoke("POST", "StatusProcess/Remove", req);
        }
    }
}