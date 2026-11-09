using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZInventoryOutputSearchListRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZInventoryOutputSearchListResponse
    {
        [DataMember]
        public int InventoryOutputID { get; set; }
        [DataMember]
        public string InventoryOutputCode { get; set; }
        [DataMember]
        public ZOrderResponse Order { get; set; }
        [DataMember]
        public ZBaseResponse Status { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string CreatedName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }
}
