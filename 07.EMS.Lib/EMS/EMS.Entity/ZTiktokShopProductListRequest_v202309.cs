using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductListRequest_v202309
    {
        public int page_size { get; set; }
        //public string page_token { get; set; }
        //public string status { get; set; }
        //public string[] seller_sku { get; set; }
        //public long create_time_ge { get; set; }
        //public long create_time_le { get; set; }
        //public long update_time_ge { get; set; }
        //public long update_time_le { get; set; }

    }
    public class ZTiktokShopProductListResponse_v202309
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopProductListDataResponse_v202309 Data { get; set; }
    }
    public class ZTiktokShopProductListDataResponse_v202309
    {

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("products")]
        public List<ZTiktokShopProductListDataProductsResponse_v202309> Products { get; set; }

        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }
    }
    public class ZTiktokShopProductListDataProductsResponse_v202309
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("skus")]
        public List<ZTiktokShopProductListDataProductsSKUSResponse_v202309> SKUS { get; set; }

        [JsonProperty("sale_regions")]
        public List<string> SaleRegions { get; set; }

        [JsonProperty("create_time")]
        public long CreateTime { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }

        [JsonProperty("product_sync_fail_reasons")]
        public List<string> ProductSyncFailReasons { get; set; }
    }
    public class ZTiktokShopProductListDataProductsSKUSResponse_v202309
    {

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSku { get; set; }

        [JsonProperty("price")]
        public ZTiktokShopProductListDataProductsSKUSPriceResponse_v202309 Price { get; set; }

        [JsonProperty("inventory")]
        public List<ZTiktokShopProductListDataProductsSKUSInventoryResponse_v202309> Inventory { get; set; }
    }
    public class ZTiktokShopProductListDataProductsSKUSPriceResponse_v202309
    {

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("tax_exclusive_price")]
        public string TaxExclusivePrice { get; set; }

        [JsonProperty("sale_price")]
        public string SalePrice { get; set; }
    }
    public class ZTiktokShopProductListDataProductsSKUSInventoryResponse_v202309
    {
        [JsonProperty("warehouse_id")]
        public string WarehouseID { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
