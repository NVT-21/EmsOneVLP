using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderCancelStatusRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ReasonCancel { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderCancelStatusResponse
    {
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
