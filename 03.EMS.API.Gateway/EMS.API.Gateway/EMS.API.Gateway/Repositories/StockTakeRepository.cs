using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class StockTakeRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStockTakeSearchListRequest>(data);
            return XHelper.Invoke("POST", "StockTake/List", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStockTakeAddRequest>(data);
            return XHelper.Invoke("POST", "StockTake/Add", req);
        }
        public static ZResponse Edit(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStockTakeEditRequest>(data);
            return XHelper.Invoke("POST", "StockTake/Edit", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStockTakeGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("StockTake/GetByID?ID={0}&ShopID={1}", req.ID, req.ShopID), null);
        }
        //public static ZResponse Detail(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZStockTakeDetailRequest>(data);
        //    return XHelper.Invoke("POST", "StockTake/Detail", req);
        //}
        public static ZResponse Accept(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStockTakeAcceptRequest>(data);
            return XHelper.Invoke("POST", "StockTake/Accept", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStockTakeRemoveRequest>(data);
            return XHelper.Invoke("POST", "StockTake/Remove", req);
        }
        public static ZResponse Reject(string data)
        {
            var req = JsonConvert.DeserializeObject<ZStockTakeRejectRequest>(data);
            return XHelper.Invoke("POST", "StockTake/Reject", req);
        }
    }
}