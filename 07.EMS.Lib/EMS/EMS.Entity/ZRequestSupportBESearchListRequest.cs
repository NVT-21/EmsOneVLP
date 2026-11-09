using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportBESearchListRequest
    {
        [DataMember]
        public int TypeDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string RequestSupportCode { get; set; }
        [DataMember]
        public string EShippingCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string RequestSupportGroup { get; set; }
        [DataMember]
        public string RequestSupportType { get; set; }
        [DataMember]
        public string RequestSupportStatus { get; set; }
        [DataMember]
        public int UserID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportBESearchListResponse
    {
        [DataMember]
        public int RequestSupportID { get; set; }
        [DataMember]
        public string RequestSupportCode { get; set; }
        [DataMember]
        public string EShippingCode { get; set; }
        [DataMember]
        public ZBaseResponse RequestSupportType { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public ZBaseResponse RequestSupportStatus { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }

        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }

        [DataMember]
        public ZBaseResponse RequestSupportGroup { get; set; }
    }
}
