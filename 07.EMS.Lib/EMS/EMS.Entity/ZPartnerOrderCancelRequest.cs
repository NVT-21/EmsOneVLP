using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerOrderCancelRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; } = "";
        [DataMember]
        public string CustomerToken { get; set; } = "";
        [DataMember]
        public string OrderCode { get; set; } = "";
        [DataMember]
        public string ShippingCode { get; set; } = "";
        [DataMember]
        public string ReasonCancel { get; set; } = "";
    }
}
