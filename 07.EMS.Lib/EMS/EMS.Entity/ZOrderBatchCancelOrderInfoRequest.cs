using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchCancelOrderInfoRequest
    {
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchCancelOrderInfoResponse
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
    }
}
