using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportServiceSearchListByShopIDRequest
    {
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportServiceSearchListByShopIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string SpecialService { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public int Sort { get; set; }
        [DataMember]
        public string IsDefault { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
