using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerTransportServiceSearchListRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int ParentID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerTransportServiceSearchListResponse
    {
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public string ServiceCode { get; set; }
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        //[DataMember]
        //public string IsLock { get; set; }
        //[DataMember]
        //public int Sort { get; set; }
        [DataMember]
        public string IsDefault { get; set; }
    }
}
