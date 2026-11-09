using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class WardsRepository
    {
        public static ZResponse Gets()
        {
            return XHelper.Invoke("GET", "Ward/Gets", null);
        }
        public static ZResponse SearchWard(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWardSearchRequest>(data);
            return XHelper.Invoke("POST", "Ward/SearchWard", req);
        }
        public static ZResponse GetByDistrictID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWardGetByDistrictIDRequest>(data);
            return XHelper.Invoke("GET", "Ward/GetByDistrictID?DistrictID="+ req.DistrictID + "&Type=" + req.Type, null);
        }
        public static ZResponse GetWardIDByWardName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPDWGetIDByPDWNameRequest>(data);
            return XHelper.Invoke("GET", "Ward/GetWardIDByWardName?DistrictID="+req.ID+"&WardName="+req.Name, null);
        }
        public static ZResponse GetByVMapCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWardGetByVMapCodeRequest>(data);
            return XHelper.Invoke("GET", "Ward/GetByVMapCode?VMapCode=" + req.VMapCode, null);
        }
        public static ZResponse GetByParentName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWardGetByParentNameRequest>(data);
            return XHelper.Invoke("POST", "Ward/GetByParentName", req);
        }
    }
}