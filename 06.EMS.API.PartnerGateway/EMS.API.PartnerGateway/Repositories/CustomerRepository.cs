using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.PartnerGateway.Repositories
{
    public class CustomerRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerAddNewRequest>(data);
            return XHelper.Invoke("POST", "Customer/AddNew", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("Customer/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }
        public static ZResponse IsExist(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerIsExistRequest>(data);
            return XHelper.Invoke("POST", "Customer/IsExist", req);
        }
    }
}