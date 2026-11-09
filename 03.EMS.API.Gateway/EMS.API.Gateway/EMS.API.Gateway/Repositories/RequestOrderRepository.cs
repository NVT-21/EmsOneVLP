using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class RequestOrderRepository
    {
        public static ZResponse ListRequestOrder(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderListRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/ListRequestOrder", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderGetByIDRequest>(data);
            return XHelper.Invoke("GET", "RequestOrder/GetByID?ID=" + req.ID + "&ShopID=" + req.ShopID, null);
        }
        public static ZResponse AddNew(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderAddRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/AddNew", req);
        }
        public static ZResponse AddNewV2(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderAddV2Request>(data);
            return XHelper.Invoke("POST", "RequestOrder/AddNewV2", req);
        }
        public static ZResponse Accept(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderAcceptRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/Accept", req);
        }
        public static ZResponse ListDivision(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderDivisionRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/ListDivision", req);
        }
        public static ZResponse ExcelDivision(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderImportAddCommitRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/ExcelDivision", req);
        }
        public static ZResponse ChangeStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderRejectRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/ChangeStatus", req);
        }
        public static ZResponse ChangeReason(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderRejectRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/ChangeReason", req);
        }
        public static ZResponse ListDivisionAgain(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderDivisionAgainRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/ListDivisionAgain", req);
        }
        public static ZResponse Update(string data)
        {
            var req = JsonConvert.DeserializeObject<ZRequestOrderUpdateRequest>(data);
            return XHelper.Invoke("POST", "RequestOrder/Update", req);
        }
    }
}