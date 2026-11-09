using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class DistrictRepository
    {
        public static ZResponse GetByProvinceID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDistrictGetByProvinceIDRequest>(data);
            //return XHelper.Invoke("GET", string.Format("District/GetByProvinceID?ProvinceID={0}", req.ProvinceID), null);
            return XHelper.Invoke("POST", "Partner/DistrictGetByProvinceID", req);
        }
    }
}