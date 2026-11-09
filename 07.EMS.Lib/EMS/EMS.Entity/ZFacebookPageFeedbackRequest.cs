using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZFacebookPageFeedbackRequest
    {
        [DataMember]
        public int ScriptType { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookPageFeedbackResponse
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string PageName { get; set; }
        [DataMember]
        public string AccountToken { get; set; }
        [DataMember]
        public string PageAccessToken { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string AmndDate { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookListAutoFeedbackRequest
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public int ScriptType { get; set; }
        [DataMember]
        public int ExecutionTimePeriod { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookListAutoFeedbackRespone
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string ChannelCommentID { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string FromTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public string SenderID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZFacebookUpdateAutoFeedbackRequest
    {
        [DataMember]
        public string ChannelCommentID { get; set; }
        [DataMember]
        public int ScriptType { get; set; }
    }
}
