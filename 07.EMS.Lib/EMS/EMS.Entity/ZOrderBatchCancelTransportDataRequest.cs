using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBatchCancelTransportDataRequest
    {
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBatchCancelTransportDataResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
