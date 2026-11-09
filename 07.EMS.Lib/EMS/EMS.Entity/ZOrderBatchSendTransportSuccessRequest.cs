using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchSendTransportSuccessRequest
    {
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string AmndState { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
