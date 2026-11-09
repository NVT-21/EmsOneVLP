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
    public class ZFacebookListPageAutoHiddenResponse
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string AccountID { get; set; }
        [DataMember]
        public string AccountToken { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string TextInComment { get; set; }
        [DataMember]
        public int IsEmail { get; set; }
        [DataMember]
        public int IsPhonenumber { get; set; }
        [DataMember]
        public int IsText { get; set; }
        [DataMember]
        public int IsAll { get; set; }
        [DataMember]
        public int AvailableDate { get; set; }
        [DataMember]
        public string PageAccessToken { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookListAutoHiddenRequest
    {
        [DataMember]
        public string PageID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookListAutoHiddenResponse
    {
        [DataMember]
        public string ChannelCommentID { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookUpdateAutoHiddenRequest
    {
        [DataMember]
        public string ChannelCommentID { get; set; }
    }
}
