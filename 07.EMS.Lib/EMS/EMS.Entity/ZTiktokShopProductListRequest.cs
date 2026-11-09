using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopProductListRequest
    {
        public int search_status { get; set; }
        public string[] seller_sku_list { get; set; }
        public long create_time_from { get; set; }
        public long create_time_to { get; set; }
        public long update_time_from { get; set; }
        public long update_time_to { get; set; }
        public int page_number { get; set; }
        public int page_size { get; set; }
    }
    public class ZTiktokShopProductListResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopProductListDataResponse Data { get; set; }
    }
    public class ZTiktokShopProductListDataResponse
    {

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("products")]
        public List<ZTiktokShopProductListDataProductResponse> Products { get; set; }
    }
    public class ZTiktokShopProductListDataProductResponse
    {
        [JsonProperty("create_time")]
        public long CreateTime { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sale_regions")]
        public List<string> SaleRegions { get; set; }

        [JsonProperty("skus")]
        public List<ZTiktokShopProductListDataProductSKUResponse> SKUS { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }
    }
    public class ZTiktokShopProductListDataProductSKUResponse
    {

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("price")]
        public ZTiktokShopProductListDataProductSKUPriceResponse Price { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSku { get; set; }

        [JsonProperty("stock_infos")]
        public List<ZTiktokShopProductListDataProductSKUStockInforResponse> StockInfos { get; set; }
    }
    public class ZTiktokShopProductListDataProductSKUPriceResponse
    {

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("original_price")]
        public string OriginalPrice { get; set; }
    }
    public class ZTiktokShopProductListDataProductSKUStockInforResponse
    {

        [JsonProperty("available_stock")]
        public int AvailableStock { get; set; }

        [JsonProperty("warehouse_id")]
        public string WarehouseID { get; set; }
    }

    
}
