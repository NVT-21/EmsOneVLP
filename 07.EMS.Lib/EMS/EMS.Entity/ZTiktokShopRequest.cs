using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZTiktokShopRequest
    {
    }
    
    public class ZTiktokShopResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("request_id")]
        public string RequestID { get; set; }

        [JsonProperty("data")]
        public ZTiktokShopDataResponse Data { get; set; }
    }

    public class ZTiktokShopDataResponse
    {
        [JsonProperty("core_user_id")]
        public string CoreUserID { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarURL { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
