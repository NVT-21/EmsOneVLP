using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductVariantRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZProductVariantResponse
    {
        [DataMember]
        public int ProductVariantID { get; set; }
        [DataMember]
        public string ProductVariantName { get; set; }
        [DataMember]
        public string ProductVariantSKU { get; set; }
        [DataMember]
        public long ProductVariantPrice { get; set; }
        [DataMember]
        public string ProductVariantOptions { get; set; }
        [DataMember]
        public double ProductVariantWeight { get; set; }
        [DataMember]
        public ZPalletResponse Pallet { get; set; }
    }
}
