using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class DistrictsRepository
    {
        public static ZResponse SearchDistrict(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDistrictSearchRequest>(data);
            return XHelper.Invoke("POST", "District/SearchDistrict", req);
        }
        public static ZResponse GetByProvinceID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDistrictGetByProvinceIDRequest>(data);
            return XHelper.Invoke("GET", "District/GetByProvinceID?ProvinceID=" + req.ProvinceID, null);
        }
        public static ZResponse GetDistrictIDByDistrictName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPDWGetIDByPDWNameRequest>(data);
            return XHelper.Invoke("GET", "District/GetDistrictIDByDistrictName?ProvinceID="+req.ID+"&DistrictName="+req.Name, null);
        }
        public static ZResponse GetByVMapCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDistrictGetByVMapCodeRequest>(data);
            return XHelper.Invoke("GET", "District/GetByVMapCode?VMapCode=" + req.VMapCode, null);
        }
        public static ZResponse GetByProvinceName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDistrictGetByProvinceNameRequest>(data);
            return XHelper.Invoke("GET", "District/GetByProvinceName?ProvinceName=" + req.ProvinceName, null);
        }
    }
}