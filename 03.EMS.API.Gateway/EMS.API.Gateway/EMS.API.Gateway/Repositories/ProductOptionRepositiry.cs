using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class ProductOptionRepositiry
    {
        public static ZResponse GetNameByProductID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductOptionGetNameByProductIDRequest>(data);
            return XHelper.Invoke("POST", "ProductOption/GetNameByProductID", req);
        }
        public static ZResponse GetByProductID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductOptionGetByProductIDRequest>(data);
            return XHelper.Invoke("POST", "ProductOption/GetByProductID", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductOptionAddRequest>(data);
            return XHelper.Invoke("POST", "ProductOption/Add", req);
        }
        public static ZResponse RemoveByName(string data)
        {
            var req = JsonConvert.DeserializeObject<ZProductOptionRemoveByNameRequest>(data);
            return XHelper.Invoke("POST", "ProductOption/RemoveByName", req);
        }
    }
}