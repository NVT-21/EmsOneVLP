using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialWebhook.Models
{
    public class TiktokRequest
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("shop_id")]
        public string ShopID { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }

    public class TiktokReceiveStatusRequest
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("shop_id")]
        public string ShopID { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("data")]
        public TiktokOrderStatusUpdate Data { get; set; }
    }
    public class TiktokOrderStatusUpdate
    {
        [JsonProperty("order_id")]
        public string OrderID { get; set; }
        [JsonProperty("order_status")]
        public string OrderStatus { get; set; }
        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }
    }

    public class TiktokReverseStatusRequest
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("shop_id")]
        public string ShopID { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("data")]
        public TiktokReverseStatusUpdate Data { get; set; }
    }
    public class TiktokReverseStatusUpdate
    {
        [JsonProperty("order_id")]
        public string OrderID { get; set; }
        [JsonProperty("reverse_type")]
        public int ReverseType { get; set; }
        [JsonProperty("reverse_user")]
        public int ReverseUser { get; set; }
        [JsonProperty("reverse_order_status")]
        public int ReverseOrderStatus { get; set; }
        [JsonProperty("reverse_event_type")]
        public string ReverseEventType { get; set; }
        [JsonProperty("reverse_order_id")]
        public string ReverseOrderID { get; set; }
        [JsonProperty("update_time")]
        public long UpdateTime { get; set; }
    }

    public class TiktokSellerDeauthorizationRequest
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("shop_id")]
        public string ShopID { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("data")]
        public TiktokSellerDeauthorization Data { get; set; }
    }
    public class TiktokSellerDeauthorization
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class TiktokAuthExpireRequest
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("shop_id")]
        public string ShopID { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("data")]
        public TiktokAuthExpire Data { get; set; }
    }
    public class TiktokAuthExpire
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("expiration_time")]
        public long ExpirationTime { get; set; }
    }
}