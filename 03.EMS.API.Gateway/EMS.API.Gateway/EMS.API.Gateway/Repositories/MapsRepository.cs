using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class MapsRepository
    {
        public static ZResponse GetByVMapCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZMapsGetByVMapCodeRequest>(data);
            return XHelper.Invoke("POST", "Maps/GetByVMapCode", req);
        }
        public static ZResponse GetByPWDName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZMapsGetByPWDNameRequest>(data);
            return XHelper.Invoke("POST", "Maps/GetByPWDName", req);
        }
        public static ZResponse GetByMBC2017Code(string data)
        {
            var req = JsonConvert.DeserializeObject<ZMapsGetByMBC2017CodeRequest>(data);
            return XHelper.Invoke("POST", "Maps/GetByMBC2017Code", req);
        }

        public static ZResponse GetByMBCCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZMapsGetByMBC2017CodeRequest>(data);
            return XHelper.Invoke("POST", "Maps/GetByMBCCode", req);
        }


    }
}