using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZWareHouseRequest
    {
    }

    [Serializable]
    [DataContract]
    public class ZWareHouseResponse
    {
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public string WareHouseCode { get; set; }
        [DataMember]
        public string WareHouseName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }
}
