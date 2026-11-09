using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class GrabOrderRepository
    {
        public static ZResponse GrabDeliveryAddOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderAddRequest>(data);
            req.isSendTransport = "Y";
            return XHelper.Invoke("POST", "GrabOrder/GrabDeliveryAddOrder", req);
        }
        public static ZResponse GrabDeliveryCancelOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZOrderCancelStatusRequest>(data);
            return XHelper.Invoke("POST", "GrabOrder/GrabDeliveryCancelOrder", req);
        }
        public static ZResponse GrabDeliveryListOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZGrabOrderSearchListRequest>(data);
            return XHelper.Invoke("POST", "GrabOrder/GrabDeliveryListOrder", req);
        }
    }
}