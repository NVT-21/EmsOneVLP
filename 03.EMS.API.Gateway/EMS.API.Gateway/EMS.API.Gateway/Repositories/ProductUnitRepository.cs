using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ProductUnitRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductUnitSearchListRequest>(data);
            return XHelper.Invoke("POST", "ProductUnit/SearchListUnit", req);
        }

        public static ZResponse AddUnit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductUnitAddRequest>(data);
            return XHelper.Invoke("POST", "ProductUnit/AddUnit", req);
        }
    }
}