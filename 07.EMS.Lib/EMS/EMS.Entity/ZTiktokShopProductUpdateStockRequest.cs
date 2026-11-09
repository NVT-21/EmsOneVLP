using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductUpdateStockRequest
    {
        public string product_id { get; set; }
        public List<ZTiktokShopProductUpdateStockSKUSRequest> skus { get; set; }
    }
    public class ZTiktokShopProductUpdateStockSKUSRequest
    {
        public string id { get; set; }
        public List<ZTiktokShopProductUpdateStockSKUSStockInfosRequest> stock_infos { get; set; }
    }
    public class ZTiktokShopProductUpdateStockSKUSStockInfosRequest
    {
        public string warehouse_id { get; set; }
        public int available_stock { get; set; }
    }
    public class ZTiktokShopProductUpdateStockResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopProductUpdateStockDataResponse Data { get; set; }
    }
    public class ZTiktokShopProductUpdateStockDataResponse
    {
        [JsonProperty("failed_skus")]
        public List<ZTiktokShopProductUpdateStockDataFailedSKUSResponse> FailedSKUS { get; set; }
    }
    public class ZTiktokShopProductUpdateStockDataFailedSKUSResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("failed_warehouse_ids")]
        public string[] FailedWareHouseIDS { get; set; }
    }
}
