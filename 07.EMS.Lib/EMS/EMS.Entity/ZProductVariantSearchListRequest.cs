using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductVariantSearchListRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZProductVariantSearchListResponse
    {
        [DataMember]
        public ZProductResponse Product { get; set; }
    }
}
