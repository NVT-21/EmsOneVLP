using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace SocialWebhook.Models
{
    public class FacebookRequest
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<FacebookEntryRequest> Entry { get; set; }

    }
    public class FacebookEntryRequest
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("messaging")]
        public List<FacebookMessagingRequest> Messaging { get; set; }
    }
    public class FacebookMessagingRequest
    {
        [JsonProperty("sender")]
        public FacebookSenderRequest Sender { get; set; }

        [JsonProperty("recipient")]
        public FacebookRecipientRequest Recipient { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("message")]
        public FacebookMessageRequest Message { get; set; }

        [JsonProperty("read")]
        public FacebookReadRequest Read { get; set; }

        [JsonProperty("delivery")]
        public FacebookDeliveryRequest Delivery { get; set; }
    }
    public class FacebookSenderRequest
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }
    public class FacebookRecipientRequest
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }
    public class FacebookMessageRequest
    {
        [JsonProperty("mid")]
        public string MID { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public List<FacebookMessageAttachmentRequest> Attachments { get; set; }
    }
    public class FacebookReadRequest
    {
        [JsonProperty("watermark")]
        public string Watermark { get; set; }
    }
    public class FacebookDeliveryRequest
    {
        [JsonProperty("mids")]
        public List<string> MIDs { get; set; }

        [JsonProperty("watermark")]
        public string Watermark { get; set; }
    }
    public class FacebookMessageAttachmentRequest
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("payload")]
        public FacebookMessageAttachmentPayloadRequest Payload { get; set; }
    }
    public class FacebookMessageAttachmentPayloadRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
    public class hub
    {
        public string mode { get; set; }
        public string verify_token { get; set; }
        public int challenge { get; set; }
    }
}