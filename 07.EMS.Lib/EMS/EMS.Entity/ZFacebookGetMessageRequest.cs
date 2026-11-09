using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookGetMessageRequest
    {
        public string ConversationID { get; set; }
        public string PageAccessToken { get; set; }
        public string PageAfter { get; set; }
    }
    public class ZFacebookGetMessageResponse
    {
        [JsonProperty("messages")]
        public ZFacebookGetMessagesResponse Messages { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
    }
    public class ZFacebookGetMessagesResponse
    {
        [JsonProperty("data")]
        public List<ZFacebookGetMessagesDataResponse> Data { get; set; }
        [JsonProperty("paging")]
        public ZFacebookGetMessagesPagingResponse Paging { get; set; }
    }
    public class ZFacebookGetMessagesDataResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty("from")]
        public ZFacebookGetMessagesDataFromResponse From { get; set; }
        [JsonProperty("to")]
        public ZFacebookGetMessagesDataToResponse To { get; set; }
    }
    public class ZFacebookGetMessagesDataFromResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
    public class ZFacebookGetMessagesDataToResponse
    {
        [JsonProperty("data")]
        public List<ZFacebookGetMessagesDataToDataResponse> Data { get; set; }
    }
    public class ZFacebookGetMessagesDataToDataResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
    public class ZFacebookGetMessagesPagingResponse
    {
        [JsonProperty("cursors")]
        public ZFacebookGetMessagesPagingCursorsResponse Cursors { get; set; }
    }
    public class ZFacebookGetMessagesPagingCursorsResponse
    {
        [JsonProperty("before")]
        public string Before { get; set; }
        [JsonProperty("after")]
        public string After { get; set; }
    }
}
