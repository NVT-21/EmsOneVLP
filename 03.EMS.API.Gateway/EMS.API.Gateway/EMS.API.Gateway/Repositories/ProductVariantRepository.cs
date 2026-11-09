using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ProductVariantRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantSearchListRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/SearchList", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantGetByIDRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/GetByID", req);
        }
        public static ZResponse GetByProductID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantGetByProductIDRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/GetByProductID", req);
        }
        public static ZResponse GetByIDs(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantGetByIDsRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/GetByIDs", req);
        }
        public static ZResponse GetBySKU(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantGetBySKURequest>(data);
            return XHelper.Invoke("GET", "ProductVariant/GetBySKU?SKU=" + req.SKU + "&ShopID=" + req.ShopID, null);
        }
        public static ZResponse GetDetailByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantGetDetailByIDRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/GetDetailByID", req);
        }
        public static ZResponse DataByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantDataByIDRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/DataByID", req);
        }
        public static ZResponse GetByShopID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantGetByShopIDRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/GetByShopID", req);
        }
        public static ZResponse UpdateImage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantUpdateImageRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/UpdateImage", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantAddRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/Add", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantEditRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/Edit", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantRemoveRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/Remove", req);
        }
        public static ZResponse GetByWareHouseID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantGetByWareHouseIDRequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/GetByWareHouseID", req);
        }
        public static ZResponse CheckSKU(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductVariantCheckSKURequest>(data);
            return XHelper.Invoke("POST", "ProductVariant/CheckSKU", req);
        }
    }
}