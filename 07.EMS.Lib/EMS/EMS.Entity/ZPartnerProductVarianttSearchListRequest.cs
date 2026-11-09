using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerProductVarianttSearchListRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZPartnerProductVariantSearchListResponse
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public string ProductVariantName { get; set; }
        [DataMember]
        public string ProductVariantImage { get; set; }
        [DataMember]
        public long ProductVariantPrice { get; set; }
        [DataMember]
        public string ProductVariantSKU { get; set; }
        [DataMember]
        public string ProductVariantBarcode { get; set; }
        [DataMember]
        public int ProductVariantInStock { get; set; }
        [DataMember]
        public double ProductVariantWeight { get; set; }
    }
}
