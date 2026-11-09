using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendEMSRequest
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        [DataMember]
        public string RequestSupportCode { get; set; }
        [DataMember]
        public string AcceptancePosCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string UpdateInformation { get; set; }
        [DataMember]
        public string TransportExtraService { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
