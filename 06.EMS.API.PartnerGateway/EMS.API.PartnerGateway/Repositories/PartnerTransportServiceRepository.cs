using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class PartnerTransportServiceRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPartnerTransportServiceSearchListRequest>(data);
            return XHelper.Invoke("POST", "Partner/TransportServiceSearchList", req);
        }
    }
}