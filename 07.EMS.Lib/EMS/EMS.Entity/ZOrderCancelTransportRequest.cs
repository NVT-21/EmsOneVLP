using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderCancelTransportRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderCancelTransportResponse
    {
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
