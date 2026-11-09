using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class WarehouseProductRepository
    {
        public static ZResponse Update(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseProductUpdateRequest>(data);
            return XHelper.Invoke("POST", "WarehouseProduct/Update", req);
        }
        public static ZResponse GetByVariantID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseProductGetByVariantIDRequest>(data);
            return XHelper.Invoke("POST", "WarehouseProduct/GetByVariantID", req);
        }
    }
}