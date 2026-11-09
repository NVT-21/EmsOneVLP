using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZMessageAddSyncRequest
    {
        public string PageID { get; set; }
        public string ChannelMessageID { get; set; }
        public int FromWhom { get; set; }
        public string SenderID { get; set; }
        public string RecipientID { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }

    }

    public class MessageFromFacebook
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

    public class MessageToFacebook
    {
        public List<MessageToFacebookData> data { get; set; }
    }

    public class MessageToFacebookData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

    public class ZMessageFacebookResponse
    {
        public List<ZMessageFacebookResponseData> data { get; set; }
        //public string PageID { get; set; }
        //public string ChannelMessageID { get; set; }
        //public int FromWhom { get; set; }
        //public string SenderID { get; set; }
        //public string RecipientID { get; set; }
        //public string Message { get; set; }
        //public string Channel { get; set; }
        //public string CreatedBy { get; set; }
        //public string CreatedDate { get; set; }
        //public string UpdatedBy { get; set; }
        //public string UpdatedDate { get; set; }
    }

    public class ZMessageFacebookResponseData
    {
        public string id { get; set; }
        public string created_time { get; set; }
        public int message { get; set; }
        public MessageFromFacebook from { get; set; }
        public MessageToFacebook to { get; set; }
    }
}
