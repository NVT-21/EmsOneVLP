using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ProductGroupRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductGroupListRequest>(data);
            return XHelper.Invoke("POST", "ProductGroup/List", req);
        }

        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductGroupAddRequest>(data);
            return XHelper.Invoke("POST", "ProductGroup/Add", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductGroupGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("ProductGroup/GetByID?ProductGroupID={0}&ShopID={1}", req.ProductGroupID, req.ShopID), null);
        }

        public static ZResponse GetByCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductGroupGetByCodeRequest>(data);
            return XHelper.Invoke("POST", "ProductGroup/GetByCode", req);
        }

        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductGroupEditRequest>(data);
            return XHelper.Invoke("POST", "ProductGroup/Edit", req);
        }

        public static ZResponse SearchListProduct(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductGroupSearchListProductRequest>(data);
            return XHelper.Invoke("POST", "ProductGroup/SearchListProduct", req);
        }
    }
}