using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportSearchInfoSendMailRequest
    {
        [DataMember]
        public string ShopFullName { get; set; }
        [DataMember]
        public string CrmOrPayPostCode { get; set; }
        [DataMember]
        public string EmailBCG { get; set; }
        [DataMember]
        public string EmailCSKH { get; set; }
        [DataMember]
        public string RequestSupportCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public ZBaseResponse RequestSupportGroup { get; set; }
        public ZBaseResponse RequestSupportType { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
