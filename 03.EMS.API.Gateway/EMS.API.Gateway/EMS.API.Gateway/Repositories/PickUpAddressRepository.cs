using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using Newtonsoft.Json;

namespace EMS.API.Gateway.Repositories
{
    public class PickUpAddressRepository
    {
        public static ZResponse SearchList(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPickUpAddressSearchListRequest>(data);
            return XHelper.Invoke("POST", "PickUpAddress/SearchList", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPickUpAddressAddRequest>(data);
            return XHelper.Invoke("POST", "PickUpAddress/Add", req);
        }
        //public static ZResponse SearchListTree(string data)
        //{
        //    var req = JsonConvert.DeserializeObject<ZPickUpAddressSearchListRequest>(data);
        //    return XHelper.Invoke("POST", "PickUpAddress/SearchListTree", req);
        //}
        public static ZResponse GetByID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPickUpAddressGetByIDRequest>(data);
            return XHelper.Invoke("GET", string.Format("PickUpAddress/GetByID?ShopID={0}&PickUpAddressID={1}", req.ShopID, req.PickUpAddressID), null);
        }
        public static ZResponse Update(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPickUpAddressUpdateRequest>(data);
            return XHelper.Invoke("POST", "PickUpAddress/Update", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPickUpAddressRemoveRequest>(data);
            return XHelper.Invoke("POST", "PickUpAddress/Remove", req);
        }
    }
}