using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.Gateway.Repositories
{
    public class TransportPostageDetailRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageDetailAddRequest>(data);
            return XHelper.Invoke("POST", "TransportPostageDetail/Add", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageDetailEditRequest>(data);
            return XHelper.Invoke("POST", "TransportPostageDetail/Edit", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageDetailRemoveRequest>(data);
            return XHelper.Invoke("POST", "TransportPostageDetail/Remove", req);
        }
        public static ZResponse GetByTransportPostageID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportPostageDetailGetByTransportPostageIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("TransportPostageDetail/GetByTransportPostageID?TransportPostageID={0}", req.TransportPostageID), null);
        }
    }
}