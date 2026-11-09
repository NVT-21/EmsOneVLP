using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductGroupSearchListProductRequest
    {
        [DataMember]
        public int ProductGroupID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZProductGroupSearchListProductResponse
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public long ProductPriceSell { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
