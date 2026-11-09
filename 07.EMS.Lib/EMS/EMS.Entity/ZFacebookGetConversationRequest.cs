using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookGetConversationRequest
    {
        public string PageID { get; set; }
        public string PageAccessToken { get; set; }
    }
    public class ZFacebookGetConversationResponse
    {
        [JsonProperty("conversations")]
        public ZFacebookGetConversationsResponse Conversations { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
    }
    public class ZFacebookGetConversationsResponse
    {
        [JsonProperty("data")]
        public List<ZFacebookGetConversationsDataResponse> Data { get; set; }
        [JsonProperty("paging")]
        public ZFacebookGetConversationsPagingResponse Paging { get; set; }
    }
    public class ZFacebookGetConversationsDataResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("senders")]
        public ZFacebookGetConversationsDataSendersResponse Senders { get; set; }
        [JsonProperty("updated_time")]
        public DateTime UpdatedTime { get; set; }
    }
    public class ZFacebookGetConversationsDataSendersResponse
    {
        [JsonProperty("data")]
        public List<ZFacebookGetConversationsDataSendersDataResponse> Data { get; set; }
    }
    public class ZFacebookGetConversationsDataSendersDataResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
    public class ZFacebookGetConversationsPagingResponse
    {
        [JsonProperty("cursors")]
        public ZFacebookGetConversationsPagingCursorsResponse Cursors { get; set; }
    }
    public class ZFacebookGetConversationsPagingCursorsResponse
    {
        [JsonProperty("before")]
        public string Before { get; set; }
        [JsonProperty("after")]
        public string After { get; set; }
    }
}
