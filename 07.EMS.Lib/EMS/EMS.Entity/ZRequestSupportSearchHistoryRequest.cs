using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchHistoryRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchHistoryResponse
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ResultDocument { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
