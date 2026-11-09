using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopOrderListRequest
    {
        public int start_time { get; set; }
        public int end_time { get; set; }
        public int order_status { get; set; }
        public int page_size { get; set; }
        public string sort_by { get; set; }
        public string cursor { get; set; }
        public int create_time_from { get; set; }
        public int create_time_to { get; set; }
        public int update_time_from { get; set; }
        public int update_time_to { get; set; }
        public int sort_type { get; set; }
        public int delivery_option_type { get; set; }
        public string buyer_uid { get; set; }
    }
    public class ZTiktokShopOrderListResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopOrderListDataResponse Data { get; set; }
    }
    public class ZTiktokShopOrderListDataResponse
    {
        [JsonProperty("more")]
        public bool More { get; set; }

        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("order_list")]
        public List<ZTiktokShopOrderListDataOrderListResponse> OrderList { get; set; }
    }
    public class ZTiktokShopOrderListDataOrderListResponse
    {
        [JsonProperty("order_id")]
        public string OrderID { get; set; }

        [JsonProperty("order_status")]
        public int OrderStatus { get; set; }

        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }
    }
}
