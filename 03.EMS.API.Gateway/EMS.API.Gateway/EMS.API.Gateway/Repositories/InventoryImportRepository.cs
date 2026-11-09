using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class InventoryImportRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryImportSearchListRequest>(data);
            return XHelper.Invoke("POST", "InventoryImport/SearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryImportAddRequest>(data);
            return XHelper.Invoke("POST", "InventoryImport/Add", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryImportGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("InventoryImport/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryImportEditRequest>(data);
            return XHelper.Invoke("POST", "InventoryImport/Edit", req);
        }
        public static ZResponse Accept(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryImportAcceptRequest>(data);
            return XHelper.Invoke("POST", "InventoryImport/Accept", req);
        }
        public static ZResponse Reject(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryImportRejectRequest>(data);
            return XHelper.Invoke("POST", "InventoryImport/Reject", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryImportRemoveRequest>(data);
            return XHelper.Invoke("POST", "InventoryImport/Remove", req);
        }
    }
}