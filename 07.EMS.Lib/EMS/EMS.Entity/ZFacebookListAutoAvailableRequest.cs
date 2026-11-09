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
    public class ZFacebookListAutoAvailableRequest
    {
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public int AvailableDate { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookListAutoAvailableResponse
    {
        [DataMember]
        public string ChannelCommentID { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookUpdateAutoAvailableRequest
    {
        [DataMember]
        public string ChannelCommentID { get; set; }
    }
}
