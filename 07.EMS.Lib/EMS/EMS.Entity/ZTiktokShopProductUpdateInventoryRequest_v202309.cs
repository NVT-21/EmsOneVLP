using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductUpdateInventoryRequest_v202309
    {
        public List<ZTiktokShopProductUpdateInventorySKUSRequest_v202309> skus { get; set; }
    }
    public class ZTiktokShopProductUpdateInventorySKUSRequest_v202309
    {
        public string id { get; set; }
        public List<ZTiktokShopProductUpdateInventorySKUSInventoryRequest_v202309> inventory { get; set; }
    }
    public class ZTiktokShopProductUpdateInventorySKUSInventoryRequest_v202309
    {
        public string warehouse_id { get; set; }
        public int quantity { get; set; }
    }

    public class ZTiktokShopProductUpdateInventoryResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopProductUpdateInventoryDataResponse_v202309 Data { get; set; }
    }
    public class ZTiktokShopProductUpdateInventoryDataResponse_v202309
    {
        [JsonProperty("errors")]
        public List<ZTiktokShopProductUpdateInventoryDataErrorsResponse_v202309> Errors { get; set; }
    }
    public class ZTiktokShopProductUpdateInventoryDataErrorsResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("detail")]
        public ZTiktokShopProductUpdateInventoryDataErrorsDetailResponse_v202309 Detail { get; set; }
    }
    public class ZTiktokShopProductUpdateInventoryDataErrorsDetailResponse_v202309
    {
        [JsonProperty("sku_id")]
        public string SKUID { get; set; }
        [JsonProperty("extra_errors")]
        public List<ZTiktokShopProductUpdateInventoryDataErrorsDetailExtraErrorsResponse_v202309> ExtraErrors { get; set; }
    }
    public class ZTiktokShopProductUpdateInventoryDataErrorsDetailExtraErrorsResponse_v202309
    {
        [JsonProperty("warehouse_id")]
        public string WarehouseID { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
