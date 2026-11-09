using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZWareHouseAddNewRequest
    {
        [DataMember]
        public string WareHouseChannel { get; set; }
        [DataMember]
        public string WareHouseCode { get; set; }
        [DataMember]
        public string WareHouseName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string IsDefault { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }
}
