using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerSearchListRequest
    {
        [DataMember]
        public string IsLock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerSearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string PartnerCode { get; set; }
        [DataMember]
        public string PartnerName { get; set; }
    }
}
