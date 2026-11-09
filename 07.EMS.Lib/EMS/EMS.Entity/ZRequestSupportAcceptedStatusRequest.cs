using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportAcceptedStatusRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string UserCMS { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
