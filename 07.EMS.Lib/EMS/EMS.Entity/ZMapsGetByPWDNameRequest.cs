using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZMapsGetByPWDNameRequest
    {
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public string WardName { get; set; }
    }
    public class ZMapsGetByPWDNameResponse
    {
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
    }
}
