using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookGetFeedRequest
    {
        public string PageID { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookGetFeedResponse
    {
        [JsonProperty("data")]
        public List<ZFacebookGetFeedDataResponse> Data { get; set; }
    }
    public class ZFacebookGetFeedDataResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty("from")]
        public ZFacebookGetFeedDataFromResponse From { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("full_picture")]
        public string FullPicture { get; set; }
    }
    public class ZFacebookGetFeedDataFromResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class ZFacebookDeleteFeedRequest
    {
        public string FeedID { get; set; }
        public string PageAccessToken { get; set; }
    }
}
