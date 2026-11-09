using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class TemplatePrintRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTemplatePrintSearchListRequest>(data);
            return XHelper.Invoke("POST", "TemplatePrint/SearchList", req);
        }
        public static ZResponse UpdateRole(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTemplatePrintUpdateRoleRequest>(data);
            return XHelper.Invoke("POST", "TemplatePrint/UpdateRole", req);
        }
    }
}