using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductUpdatePriceRequest_v202309
    {
        public List<ZTiktokShopProductUpdatePriceSKUSRequest_v202309> skus { get; set; }
    }
    public class ZTiktokShopProductUpdatePriceSKUSRequest_v202309
    {
        public string id { get; set; }
        public ZTiktokShopProductUpdatePriceSKUSPriceRequest_v202309 price { get; set; }
    }
    public class ZTiktokShopProductUpdatePriceSKUSPriceRequest_v202309
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class ZTiktokShopProductUpdatePriceResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}
