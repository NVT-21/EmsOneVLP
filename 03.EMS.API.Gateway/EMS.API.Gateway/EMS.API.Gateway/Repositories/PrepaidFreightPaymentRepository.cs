using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class PrepaidFreightPaymentRepository
    {
        public static ZResponse ListPrepaidFreightPayment(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPrepaidFreightPaymentRequest>(data);
            return XHelper.Invoke("POST", "PrepaidFreightPayment/ListPrepaidFreightPayment", req);
        }
    }
}