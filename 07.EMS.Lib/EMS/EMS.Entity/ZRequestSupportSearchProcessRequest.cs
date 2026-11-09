using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchProcessRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchProcessResponse
    {
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Result { get; set; }
        [DataMember]
        public string ResultDocument { get; set; }
        [DataMember]
        public ZBaseResponse Type { get; set; }
    }
}
