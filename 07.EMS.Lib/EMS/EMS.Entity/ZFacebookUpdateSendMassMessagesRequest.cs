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
    public class ZFacebookUpdateSendMassMessagesRequest
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public int IsUpdate { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookSendMassMessagesRequest
    {
        [DataMember]
        public string PageID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZFacebookSendMassMessagesResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string PageID { get; set; }
        [DataMember]
        public string FacebookID { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
