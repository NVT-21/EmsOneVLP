using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZDistrictListRequest
    {
        [DataMember]
        public int ProvinceID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZDistrictListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
