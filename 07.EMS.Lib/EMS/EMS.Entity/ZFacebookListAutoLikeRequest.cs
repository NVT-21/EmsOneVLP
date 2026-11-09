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
    public class ZFacebookListAutoLikeRequest
    {
        [DataMember]
        public string PageID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZFacebookListAutoLikeResponse
    {
        [DataMember]
        public string ChannelCommentID { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZFacebookUpdateAutoLikeRequest
    {
        [DataMember]
        public string ChannelCommentID { get; set; }
    }
}
