using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderCancelOrderInfoRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderCancelOrderInfoResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string ShippingID { get; set; }
    }
}
