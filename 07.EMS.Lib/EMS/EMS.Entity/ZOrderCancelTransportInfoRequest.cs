using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderCancelTransportInfoRequest
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
        public string AmndState { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderCancelTransportInfoResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public string ShippingID { get; set; }
    }
}
