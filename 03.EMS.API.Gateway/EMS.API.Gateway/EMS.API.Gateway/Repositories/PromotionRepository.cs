using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class PromotionRepository
    {
        public static ZResponse AddNewPromotionCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPromotionAddNewCodeRequest>(data);
            //var req = JsonConvert.DeserializeObject<ZPromotionAddNewCodeRequest>(data);
            return XHelper.Invoke("POST", "Promotion/AddNewPromotionCode", req);
        }
        public static ZResponse AddNewPromotion(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPromotionAddNewRequest>(data);
            return XHelper.Invoke("POST", "Promotion/AddNewPromotion", req);
        }
        public static ZResponse GetByCode(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPromotionGetByCodeRequest>(data);
            return XHelper.Invoke("POST", "Promotion/GetByCode", req);
        }
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPromotionListRequest>(data);
            return XHelper.Invoke("POST", "Promotion/List", req);
        }
        public static ZResponse IsLock(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPromotionIsLockRequest>(data);
            return XHelper.Invoke("POST", "Promotion/IsLock", req);
        }
        public static ZResponse PromotionRemove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZPromotionDeleteRequest>(data);
            return XHelper.Invoke("POST", "Promotion/Remove", req);
        }
    }
}