using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderPaginationRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
        [DataMember]
        public int PageLimit { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZOrderPaginationResponse
    {
        [DataMember]
        public int TotalRow { get; set; }
        [DataMember]
        public List<ZOrderPaginationItemResponse> Order { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ZOrderPaginationItemResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
