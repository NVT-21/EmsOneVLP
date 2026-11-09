using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportSendRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        //[DataMember]
        //public string RequestSupportCode { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSendResponse
    {
        [DataMember]
        public string RequestSupportCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

}
