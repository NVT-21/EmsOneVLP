using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductUpdatePriceRequest
    {
        public string product_id { get; set; }
        public List<ZTiktokShopProductUpdatePriceSKUSRequest> skus { get; set; }
    }
    public class ZTiktokShopProductUpdatePriceSKUSRequest
    {
        public string id { get; set; }
        public string original_price { get; set; }
    }
    public class ZTiktokShopProductUpdatePriceResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopProductUpdatePriceDataResponse Data { get; set; }
    }
    public class ZTiktokShopProductUpdatePriceDataResponse
    {
        [JsonProperty("failed_sku_ids")]
        public List<string> FailedSKUIDS { get; set; }
    }
}
