using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerProductSearchListRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string CustomerToken { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerProductSearchListResponse
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public List<ZPartnerProductVariantSearchListResponse> ListProductVariant { get; set; }
    }
}
