using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderMultipleSendTransportRequest
    {
        [DataMember]
        public List<ZOrderMultipleSendTransportOrderInfoRequest> ListOrderInfo { get; set; } = new List<ZOrderMultipleSendTransportOrderInfoRequest>();
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderMultipleSendTransportOrderInfoRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
    }
}
