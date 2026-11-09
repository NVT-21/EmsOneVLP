using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.PartnerGateway.Repositories
{
    public class OrderRepository
    {
        public static ZResponse OrderCheckOrderCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderCheckCodeRequest>(data);
            return XHelper.Invoke("POST", "Order/CheckOrderCode", req);
        }
    }
}