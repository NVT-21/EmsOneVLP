using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ExpectedDeliveryRepository
    {
        public static ZResponse SearchDate(string data)
        {
            var req = JsonConvert.DeserializeObject<ZExpectedDeliverySearchDateRequest>(data);
            return XHelper.Invoke("POST", "ExpectedDelivery/SearchDate", req);
        }
    }
}