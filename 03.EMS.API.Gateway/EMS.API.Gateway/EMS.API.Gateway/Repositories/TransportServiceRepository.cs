using EMS.Entity;
using EMS.Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EMS.API.Gateway.Repositories
{
    public class TransportServiceRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceSearchListRequest>(data);
            return XHelper.Invoke("POST", "TransportService/SearchList", req);
        }

        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceAddRequest>(data);
            return XHelper.Invoke("POST", "TransportService/Add", req);
        }

        public static ZResponse TransportServiceEdit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceEditRequest>(data);
            return XHelper.Invoke("POST", "TransportService/Edit", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("TransportService/GetByID?ID={0}", req.ID) , null);
        }

        public static ZResponse TransportServiceLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceLockRequest>(data);
            return XHelper.Invoke("POST", "TransportService/Lock", req);
        }

        public static ZResponse TransportServiceUnlock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceUnLockRequest>(data);
            return XHelper.Invoke("POST", "TransportService/Unlock", req);
        }

        public static ZResponse TransportServiceCheckByCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceCheckByCodeRequest>(data);
            return XHelper.Invoke("POST", "TransportService/CheckByCode", req);
        }

        public static ZResponse SearchListByShopID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTransportServiceSearchListByShopIDRequest>(data);
            return XHelper.Invoke("POST", "TransportService/SearchListByShopID", req);
        }

        public static ZResponse TransportServiceEditByShop(string data)
        {
            var req = JsonConvert.DeserializeObject<ZShopTransportServiceEditRequest>(data);
            return XHelper.Invoke("POST", "TransportService/EditByShop", req);
        }
    }
}