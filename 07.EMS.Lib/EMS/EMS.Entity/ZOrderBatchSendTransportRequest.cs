using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchSendTransportRequest
    {
        [DataMember]
        public string AmndState { get; set; }
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
