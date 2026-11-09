using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderRequest
    {
       
    }

    [Serializable]
    [DataContract]
    public class ZOrderResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public ZBaseResponse OrderStatus { get; set; }
        [DataMember]
        public string IsTransport { get; set; }
        
    }
}
