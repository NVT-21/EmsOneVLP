using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace EMS.Entity
{
    public class ZFacebookConnectPageRequest
    {

    }
    public class ZFacebookConnectPageResponse
    {
        [JsonProperty("data")]
        public List<ZFacebookConnectPageDataResponse> Data { get; set; } 
    }
    public class ZFacebookConnectPageDataResponse
    {
        [JsonProperty("id")]
        public string PageID { get; set; }

        [JsonProperty("name")]
        public string PageName { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
