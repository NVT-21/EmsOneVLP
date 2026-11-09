using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class DeliveryNoteRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDeliveryNoteListRequest>(data);
            return XHelper.Invoke("POST", "DeliveryNote/List", req);
        }
        public static ZResponse GetDeliveryNoteByOrderID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDeliveryNoteByOrderIDRequest>(data);
            return XHelper.Invoke("GET", "DeliveryNote/GetDeliveryNoteByOrderID?OrderID=" + req.OrderID + "&ShopID=" + req.ShopID, null);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDeliveryNoteAddNewRequest>(data);
            return XHelper.Invoke("POST", "DeliveryNote/Add", req);
        }
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDeliveryNoteGetByIDRequest>(data);
            return XHelper.Invoke("GET", "DeliveryNote/GetByID?ID=" + req.ID, null);
        }
        public static ZResponse ChangeStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDeliveryNoteChangeStatusRequest>(data);
            return XHelper.Invoke("POST", "DeliveryNote/ChangeStatus", req);
        }
        public static ZResponse CompletedStatus(string data)
        {
            var req = JsonConvert.DeserializeObject<ZDeliveryNoteChangeStatusRequest>(data);
            return XHelper.Invoke("POST", "DeliveryNote/CompletedStatus", req);
        }
        
    }
}