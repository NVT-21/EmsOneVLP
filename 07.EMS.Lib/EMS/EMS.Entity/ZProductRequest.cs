using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZProductResponse
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public ZProductVariantResponse ProductVariant { get; set; }
        [DataMember]
        public List<ZProductVariantResponse> ListProductVariant { get; set; }
    }
}
