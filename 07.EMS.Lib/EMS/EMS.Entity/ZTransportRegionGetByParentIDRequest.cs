using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportRegionGetByParentIDRequest
    {
        [DataMember]
        public int ParentID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportRegionGetByParentIDResponse
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
        public string ProvincesID { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
}
