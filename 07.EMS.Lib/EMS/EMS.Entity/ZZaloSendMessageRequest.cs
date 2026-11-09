using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZZaloSendMessageRequest
    {
        public ZZaloSendMessageRecipientRequest recipient { get; set; }
        public ZZaloSendMessageMessageRequest message { get; set; }
        public string AccessToken { get; set; }
    }
    public class ZZaloSendMessageRecipientRequest
    {
        public string user_id { get; set; }
    }
    public class ZZaloSendMessageMessageRequest
    {
        public string text { get; set; }
        public ZZaloSendMessageAttachmentRequest attachment { get; set; }
    }
    public class ZZaloSendMessageAttachmentRequest
    {
        public string type { get; set; }
        public ZZaloSendMessagePayloadRequest payload { get; set; }
    }
    public class ZZaloSendMessagePayloadRequest
    {
        public string template_type { get; set; }
        public List<ZZaloSendMessageElementRequest> elements { get; set; }
        public string token { get; set; }
    }
    public class ZZaloSendMessageElementRequest
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string image_url { get; set; }
        public ZZaloSendMessageDefaultActionRequest default_action { get; set; }
        public string media_type { get; set; }
        public string attachment_id { get; set; }
    }
    public class ZZaloSendMessageDefaultActionRequest
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class ZZaloSendMessageResponse
    {
        public ZZaloSendMessageDataResponse data { get; set; }
        public string error { get; set; }
        public string message { get; set; }
    }
    public class ZZaloSendMessageDataResponse
    {
        public string message_id { get; set; }
        public string user_id { get; set; }
    }
}
