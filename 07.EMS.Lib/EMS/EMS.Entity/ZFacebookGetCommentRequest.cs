using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookGetCommentRequest
    {
        public string FeedID { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookGetCommentResponse
    {
        [JsonProperty("data")]
        public List<ZFacebookGetCommentDataResponse> Data { get; set; }
    }
    public class ZFacebookGetCommentDataResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty("from")]
        public ZFacebookGetCommentDataFromResponse From { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
    }
    public class ZFacebookGetCommentDataFromResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
