using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZWardGetByDistrictIDRequest
    {
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int Type { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZWardGetByProvinceIDRequest
    {
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int Type { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZWardGetByDistrictIDResponse
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
