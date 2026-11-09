using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopAccessTokenResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopAccessTokenDataResponse Data { get; set; }
    }
    public class ZTiktokShopAccessTokenDataResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("access_token_expire_in")]
        public long AccessTokenExpireIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("refresh_token_expire_in")]
        public long RefreshTokenExpireIn { get; set; }

        [JsonProperty("open_id")]
        public string OpenID { get; set; }

        [JsonProperty("seller_name")]
        public string SellerName { get; set; }

        [JsonProperty("seller_base_region")]
        public string SellerBaseRegion { get; set; }

        [JsonProperty("user_type")]
        public int UserType { get; set; }
    }
    public class ZTiktokGetShopRequest
    {
        public string app_key { get; set; }
        public string timestamp { get; set; }
        public string sign { get; set; }
        public string access_token { get; set; }

    }
    public class ZTiktokGetShopResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokGetShopDataResponse Data { get; set; }
    }
    public class ZTiktokGetShopDataResponse
    {
        [JsonProperty("shops")]
        public List<ZTiktokGetShopListDataResponse> Shops { get; set; }
    }
    public class ZTiktokGetShopListDataResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("seller_type")]
        public string SellerType { get; set; }
        [JsonProperty("cipher")]
        public string Cipher { get; set; }
    }
}
