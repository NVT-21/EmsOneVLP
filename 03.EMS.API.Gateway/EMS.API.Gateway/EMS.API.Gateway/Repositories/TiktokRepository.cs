using EMS.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.API.Gateway.Repositories
{
    public class TiktokRepository
    {
        public static ZResponse ListProduct(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTikProductListRequest>(data);
            return XHelper.Invoke("POST", "Tiktok/ListProduct", req);
        }

        public static ZResponse ListProductProcess(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTikProductProcessListRequest>(data);
            return XHelper.Invoke("POST", "Tiktok/ListProductProcess", req);
        }

        public static ZResponse ListProductNotLinkedYet(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTikProductNotLinkedYetListRequest>(data);
            return XHelper.Invoke("POST", "Tiktok/ListProductNotLinkedYet", req);
        }

        public static ZResponse ProductUpdateConnection(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTikProductUpdateConnectionRequest>(data);
            return XHelper.Invoke("POST", "Tiktok/ProductUpdateConnection", req);
        }

        public static ZResponse ConfigProductByPageID(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTikConfigProductRequest>(data);
            return XHelper.Invoke("POST", "Tiktok/ConfigProductByPageID", req);
        }

        public static ZResponse ConfigProductChange(string data)
        {
            var req = JsonConvert.DeserializeObject<ZTikConfigProductChangeRequest>(data);
            return XHelper.Invoke("POST", "Tiktok/ConfigProductChange", req);
        }
    }
}