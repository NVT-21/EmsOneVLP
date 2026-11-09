using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EMS.API.Gateway.Repositories
{
    public class WareHouseRepository
    {
        public static ZResponse AddNew(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseAddNewRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/AddNew", req);
        }

        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseSearchListRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/SearchList", req);
        }

        public static ZResponse ListProductByWareHouseID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseListProductByWareHouseIDRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/ListProductByWareHouseID", req);
        }

        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseGetByIDRequest>(data);
            return XHelper.Invoke("GET",string.Format("Warehouse/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }

        public static ZResponse ListProduct(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseListProductRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/ListProduct", req);
        }

        public static ZResponse AddInventory(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseAddInventoryRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/AddInventory", req);
        }

        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseEditRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/Edit", req);
        }
        //--------------C----------------->
        public static ZResponse GetProductsByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseGetProductsByIDRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/GetProductsByID", req);
        }
        public static ZResponse IsDefault(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseIsDefaultRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/IsDefault", req);
        }
        public static ZResponse GetDefaultWarehouse(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWarehouseGetDefaultWarehouseRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/GetDefaultWarehouse", req);
        }
        public static ZResponse ChangeIsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseChangeIsLockRequest>(data);
            return XHelper.Invoke("POST", "Warehouse/ChangeIsLock", req);
        }

        public static ZResponse GetByCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZWareHouseGetByCodeRequest>(data);
            return XHelper.Invoke("GET", string.Format("Warehouse/GetByCode?WareHouseCode={0}&ShopID={1}", req.WareHouseCode, req.ShopID), null);
        }
    }
}