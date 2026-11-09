using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZFacebookSendMessageRequest
    {
        public ZFacebookSendMessageRecipientRequest recipient { get; set; }
        public ZFacebookSendMessageMessageRequest message { get; set; }
        public string messaging_type { get; set; }
        public string tag { get; set; }
    }
    public class ZFacebookSendMessageRecipientRequest
    {
        public string id { get; set; }
    }
    public class ZFacebookSendMessageMessageRequest
    {
        public string text { get; set; }
    }

    public class ZFacebookSendFileRequest
    {
        public ZFacebookSendMessageRecipientRequest recipient { get; set; }
        public ZFacebookSendFileMessageRequest message { get; set; }
    }
    public class ZFacebookSendFileMessageRequest
    {
        public ZFacebookSendFileMessageAttachmentRequest attachment { get; set; }
    }

    public class ZFacebookSendFileMessageAttachmentRequest
    {
        public string type { get; set; }
        public ZFacebookSendFileMessageAttachmentPayloadRequest payload { get; set; }
    }
    public class ZFacebookSendFileMessageAttachmentPayloadRequest
    {
        public string url { get; set; }
        public Boolean is_reusable { get; set; }
    }
}
