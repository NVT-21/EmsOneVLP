using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderSendTransportRequest
    {
        [DataMember]
        public string AmndState { get; set; }
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
