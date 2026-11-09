using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerWareHouseSearchListRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerWareHouseSearchListResponse
    {
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public string WareHouseCode { get; set; }
        [DataMember]
        public string WareHouseName { get; set; }
        [DataMember]
        public string IsDefault { get; set; }

    }
}
