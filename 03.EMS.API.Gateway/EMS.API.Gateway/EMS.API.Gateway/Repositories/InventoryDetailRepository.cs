using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class InventoryDetailRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryDetailAddRequest>(data);
            return XHelper.Invoke("POST", "InventoryDetail/Add", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryDetailEditRequest>(data);
            return XHelper.Invoke("POST", "InventoryDetail/Edit", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryDetailRemoveRequest>(data);
            return XHelper.Invoke("POST", "InventoryDetail/Remove", req);
        }
        public static ZResponse GetByInventoryID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryDetailGetByInventoryIDRequest>(data);
            return XHelper.Invoke("POST", string.Format("InventoryDetail/GetByInventoryID?InventoryID={0}&ShopID={1}", req.InventoryID, req.ShopID), null);
        }
    }
}