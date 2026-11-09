using EMS.Entity;
using Newtonsoft.Json;
namespace EMS.API.Gateway.Repositories
{
    public class InventoryOutputRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputSearchListRequest>(data);
            return XHelper.Invoke("POST", "InventoryOutput/SearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputAddRequest>(data);
            return XHelper.Invoke("POST", "InventoryOutput/Add", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("InventoryOutput/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputEditRequest>(data);
            return XHelper.Invoke("POST", "InventoryOutput/Edit", req);
        }
        public static ZResponse Process(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputProcessRequest>(data);
            return XHelper.Invoke("POST", "InventoryOutput/Process", req);
        }
        public static ZResponse Accept(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputAcceptRequest>(data);
            return XHelper.Invoke("POST", "InventoryOutput/Accept", req);
        }
        public static ZResponse Reject(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputRejectRequest>(data);
            return XHelper.Invoke("POST", "InventoryOutput/Reject", req);
        }
        public static ZResponse Info(string data)
        {
            var req = JsonConvert.DeserializeObject<ZInventoryOutputInfoRequest>(data);
            return XHelper.Invoke("POST", "InventoryOutput/Info", req);
        }
        
    }
}