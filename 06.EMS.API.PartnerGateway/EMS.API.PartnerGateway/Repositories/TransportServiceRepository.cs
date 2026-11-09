using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class TransportServiceRepository
    {
        public static ZResponse SearchListByShopID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceSearchListByShopIDRequest>(data);
            return XHelper.Invoke("POST", "TransportService/SearchListByShopID", req);
        }
        //public static ZResponse TransportServiceCheckByCode(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZTransportServiceCheckByCodeRequest>(data);
        //    return XHelper.Invoke("POST", "TransportService/CheckByCode", req);
        //}
    }
}