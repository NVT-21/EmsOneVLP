using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZErrorDataListRequest
    {
    }

    public class ZErrorDataListResponse
    {
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string EmsBarCode { get; set; }
        [DataMember]
        public string RequestSupportCode { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string ProductVariantSKU { get; set; }
    }
}
