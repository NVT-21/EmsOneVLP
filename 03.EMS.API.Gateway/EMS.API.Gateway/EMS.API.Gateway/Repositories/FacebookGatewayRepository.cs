using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class FacebookGatewayRepository
    {
        public static ZResponse GetMessengerList(string data)
        {
            ZResponse res = new ZResponse();
            var req = JsonConvert.DeserializeObject<ZPDWGetIDByPDWNameRequest>(data);





            return res;
        }
    }
}