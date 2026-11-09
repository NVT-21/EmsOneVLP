using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class FunctionalRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZFunctionalGroupListRequest>(data);
            return XHelper.Invoke("POST", "Functional/List", req);
        }
    }
}