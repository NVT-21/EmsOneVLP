using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class WarehouseImportRepositiry
    {
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportAddRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/Add", req);
        }
        public static ZResponse AddDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportAddDetailRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/AddDetail", req);
        }
        public static ZResponse GetDataByShopID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportGetDataByShopIDRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/GetDataByShopID", req);
        }
        public static ZResponse GetDataByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportGetDataByIDRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/GetDataByID", req);
        }
        public static ZResponse GetDetailByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportGetDetailByIDRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/GetDetailByID", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportRemoveRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/Remove", req);
        }
        public static ZResponse RemoveDetail(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportRemoveDetailRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/RemoveDetail", req);
        }
        public static ZResponse Complete(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseImportCompleteRequest>(data);
            return XHelper.Invoke("POST", "WarehouseImport/Complete", req);
        }
    }
}