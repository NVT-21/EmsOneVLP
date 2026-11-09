using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class XZaloModel
    {
        public string app_id { get; set; }
        //public string oa_id { get; set; }
        public string user_id_by_app { get; set; }
        public string event_name { get; set; }
        public string timestamp { get; set; }
        //public string source { get; set; }
        public XZaloSenderModel sender { get; set; }
        public XZaloRecipientModel recipient { get; set; }
        public XZaloMessageModel message { get; set; }
        public XZaloInfoModel info { get; set; }
        public string AccessToken { get; set; }
    }
    public class XZaloSenderModel
    {
        public string id { get; set; }
    }

    public class XZaloRecipientModel
    {
        public string id { get; set; }
    }
    public class XZaloMessageModel
    {
        public string msg_id { get; set; }
        public string text { get; set; }
        public List<string> msg_ids { get; set; }
        public List<XZaloAttachmentsModel> attachments { get; set; }
        public string react_icon { get; set; }

    }
    public class XZaloAttachmentsModel
    {
        public XZaloPayloadModel payload { get; set; }
        public string type { get; set; }
    }
    public class XZaloPayloadModel
    {
        public string id { get; set; }
        public string thumbnail { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public XZaloCoordinatesModel coordinates { get; set; }
    }
    public class XZaloFollowerModel
    {
        public string id { get; set; }
        public string user_id { get; set; }
    }
    public class XZaloCoordinatesModel
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
    public class XZaloInfoModel
    {
        public string id { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string name { get; set; }
        public string ward { get; set; }
    }
    public class XZaloHostModel
    {
        public string oa_id { get; set; }
        public string ip { get; set; }
    }
    
}
