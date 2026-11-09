using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class TransportServiceRoleRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceRoleListRequest>(data);
            return XHelper.Invoke("POST", "TransportServiceRole/List", req);
        }
    }
}