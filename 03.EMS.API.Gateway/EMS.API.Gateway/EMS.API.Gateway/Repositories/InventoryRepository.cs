using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class InventoryRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryAddRequest>(data);
            return XHelper.Invoke("POST", "Inventory/Add", req);
        }
        public static ZResponse AddV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryAddRequest>(data);
            return XHelper.Invoke("POST", "Inventory/AddV2", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryEditRequest>(data);
            return XHelper.Invoke("POST", "Inventory/Edit", req);
        }
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventorySearchListRequest>(data);
            return XHelper.Invoke("POST", "Inventory/SearchList", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("Inventory/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }
        public static ZResponse Confirm(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryConfirmRequest>(data);
            return XHelper.Invoke("POST", "Inventory/Confirm", req);
        }
        public static ZResponse UpdateStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryUpdateStatusRequest>(data);
            return XHelper.Invoke("POST", "Inventory/UpdateStatus", req);
        }
        public static ZResponse Reject(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryRejectRequest>(data);
            return XHelper.Invoke("POST", "Inventory/Reject", req);
        }
        public static ZResponse Cancel(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryCancelRequest>(data);
            return XHelper.Invoke("POST", "Inventory/Cancel", req);
        }
        public static ZResponse Delete(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryDeleteRequest>(data);
            return XHelper.Invoke("POST", "Inventory/Delete", req);
        }
        public static ZResponse GetProductVariantByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryGetProductVariantByIDRequest>(data);
            return XHelper.Invoke("POST", "Inventory/GetProductVariantByID", req);
        }
    }
}