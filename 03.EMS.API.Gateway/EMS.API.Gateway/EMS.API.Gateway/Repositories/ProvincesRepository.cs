using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ProvincesRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProvinceListRequest>(data);
            return XHelper.Invoke("POST", "Province/List", req);
        }
        public static ZResponse SearchProvince(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProvinceSearchRequest>(data);
            return XHelper.Invoke("POST", "Province/SearchProvince", req);
        }
        public static ZResponse GetProvinceIDByProvinceName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPDWGetIDByPDWNameRequest>(data);
            return XHelper.Invoke("GET", "Province/GetProvinceIDByProvinceName?ProvinceName="+req.Name, null);
        }
        public static ZResponse ProvinceImportExcel(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPDWImportExcelRequest>(data);
            return XHelper.Invoke("POST", "Province/ImportExcel", req);
        }
    }
}