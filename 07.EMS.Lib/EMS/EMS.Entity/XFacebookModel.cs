using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace EMS.Entity
{
    public class XFacebookModel
    {
        public string Object { get; set; }

        public List<FacebookEntryModel> Entry { get; set; }
    }
    public class FacebookEntryModel
    {
        public string ID { get; set; }

        public string Time { get; set; }

        public List<FacebookMessagingModel> Messaging { get; set; }
    }
    public class FacebookMessagingModel
    {
        public FacebookSenderModel Sender { get; set; }

        public FacebookRecipientModel Recipient { get; set; }

        public string Timestamp { get; set; }

        public FacebookMessageModel Message { get; set; }

        public FacebookReadModel Read { get; set; }

        public FacebookDeliveryRequest Delivery { get; set; }
    }
    public class FacebookSenderModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class FacebookRecipientModel
    {
        public string ID { get; set; }
    }
    public class FacebookMessageModel
    {
        public string MID { get; set; }

        public string Text { get; set; }

        public List<FacebookMessageAttachmentModel> Attachments { get; set; }
    }
    public class FacebookMessageAttachmentModel
    {
        public string Type { get; set; }

        public FacebookMessageAttachmentPayloadModel Payload { get; set; }
    }
    public class FacebookMessageAttachmentPayloadModel
    {
        public string Url { get; set; }

        public string Title { get; set; }
    }
    public class FacebookReadModel
    {
        public string Watermark { get; set; }
    }
    public class FacebookDeliveryRequest
    {
        public string Watermark { get; set; }
    }
    public class FacebookFeedModel
    {
        public string ID { get; set; }
        public string Message { get; set; }
        public string CreatedTime { get; set; }
        public FacebookSenderModel Sender { get; set; }
    }
}
