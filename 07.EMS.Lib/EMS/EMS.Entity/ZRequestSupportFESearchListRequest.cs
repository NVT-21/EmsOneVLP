using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZRequestSupportFESearchListRequest
    {
        [DataMember]
        public int TypeDate { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string RequestGroup { get; set; }
        [DataMember]
        public string RequestType { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZRequestSupportFESearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string EShippingCode { get; set; }
        [DataMember]
        public string RequestGroup { get; set; }
        [DataMember]
        public string RequestType { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
    }
}
