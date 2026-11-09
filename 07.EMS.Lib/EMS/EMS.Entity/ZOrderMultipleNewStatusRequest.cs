using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderMultipleNewStatusRequest
    {
        [DataMember]
        public List<ZOrderMultipleNewStatusOrderInfoRequest> ListOrderInfo { get; set; } = new List<ZOrderMultipleNewStatusOrderInfoRequest>();
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderMultipleNewStatusOrderInfoRequest
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
