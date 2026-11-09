using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class RequestSupportGroupRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestSupportGroupListRequest>(data);
            return XHelper.Invoke("POST", "RequestSupportGroup/List", req);
        }
    }
}