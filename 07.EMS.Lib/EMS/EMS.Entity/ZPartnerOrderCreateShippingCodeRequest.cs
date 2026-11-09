using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerOrderCreateShippingCodeRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderCreateShippingCodeResponse
    {
        [DataMember]
        public string ShippingCode { get; set; }
    }
}
