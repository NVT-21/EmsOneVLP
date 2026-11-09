using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class WardRepository
    {
        public static ZResponse GetByDistrictID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWardGetByDistrictIDRequest>(data);
            //return XHelper.Invoke("GET", string.Format("Ward/GetByDistrictID?DistrictID={0}", req.DistrictID), null);
            return XHelper.Invoke("POST", "Partner/WardGetByDistrictID", req);
        }

        public static ZResponse GetByProvinceID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWardGetByProvinceIDRequest>(data);
            //return XHelper.Invoke("GET", string.Format("Ward/GetByDistrictID?DistrictID={0}", req.DistrictID), null);
            return XHelper.Invoke("POST", "Partner/WardGetByProvinceID", req);
        }
    }
}