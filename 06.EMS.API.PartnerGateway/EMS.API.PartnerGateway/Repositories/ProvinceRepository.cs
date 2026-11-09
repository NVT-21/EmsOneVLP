using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class ProvinceRepository
    {
        public static ZResponse SearchListProvince(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProvinceListRequest>(data);
            //return XHelper.Invoke("POST", "Province/List", req);
            return XHelper.Invoke("POST", "Partner/ProvinceSearchList", req);
        }
    }
}