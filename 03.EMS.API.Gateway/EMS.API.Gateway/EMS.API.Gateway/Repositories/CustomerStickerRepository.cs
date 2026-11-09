using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class CustomerStickerRepository
    {
        public static ZResponse List(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerStickerListRequest>(data);
            return XHelper.Invoke("POST", "CustomerSticker/List", req);
        }
        public static ZResponse Add(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerStickerAddRequest>(data);
            return XHelper.Invoke("POST", "CustomerSticker/Add", req);
        }
        public static ZResponse Remove(string data)
        {
            var req = JsonConvert.DeserializeObject<ZCustomerStickerRemoveRequest>(data);
            return XHelper.Invoke("POST", "CustomerSticker/Remove", req);
        }
    }
}