using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class ProductRepository
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductAddRequest>(data);
            return XHelper.Invoke("POST", "Product/Add", req);
        }

        public static ZResponse AddV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductAddRequest>(data);
            return XHelper.Invoke("POST", "Product/AddV2", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductGetByIDRequest>(data);
            return XHelper.Invoke("POST", "Product/GetByID", req);
        }

        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductEditRequest>(data);
            return XHelper.Invoke("POST", "Product/Edit", req);
        }

        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductListRequest>(data);
            return XHelper.Invoke("POST", "Product/List", req);
        }

        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductRemoveRequest>(data);
            return XHelper.Invoke("POST", "Product/Remove", req);
        }
        public static ZResponse DataImagesByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductDataImagesByIDRequest>(data);
            return XHelper.Invoke("POST", "Product/DataImagesByID", req);
        }
        public static ZResponse EditImages(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductEditImagesRequest>(data);
            return XHelper.Invoke("POST", "Product/EditImages", req);
        }
        public static ZResponse RemoveImage(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductRemoveRequest>(data);
            return XHelper.Invoke("POST", "Product/RemoveImage", req);
        }
        public static ZResponse UpdateAvatar(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductUpdateAvatarRequest>(data);
            return XHelper.Invoke("POST", "Product/UpdateAvatar", req);
        }
        public static ZResponse GetByVariantID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductRemoveRequest>(data);
            return XHelper.Invoke("POST", "Product/GetByVariantID", req);
        }
        public static ZResponse UpdateProductGroup(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductUpdateProductGroupRequest>(data);
            return XHelper.Invoke("POST", "Product/UpdateProductGroup", req);
        }

        public static ZResponse ImportExcel(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductImportExcelRequest>(data);
            return XHelper.Invoke("POST", "Product/ImportExcel", req);
        }
    }
}