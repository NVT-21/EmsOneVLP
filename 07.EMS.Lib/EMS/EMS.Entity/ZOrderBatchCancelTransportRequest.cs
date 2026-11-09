using System;
using System.Runtime.Serialization;
namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchCancelTransportRequest
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
