using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Facebook.Models
{
    public class FAB_GetConversationIDModel
    {
        [JsonProperty("data")]
        public List<FAB_GetConversationIDDataModel> Data { get; set; }
    }
    public class FAB_GetConversationIDDataModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("updated_time")]
        public string UpdatedTime { get; set; }
    }
}