using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZWareHouseSearchListRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZWareHouseSearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string WareHouseName { get; set; }
        [DataMember]
        public string WareHouseCode { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public string WardName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string EmployeeName { get; set; }
        [DataMember]
        public string ShopName { get; set; }
        [DataMember]
        public string IsDefault { get; set; }
        [DataMember]
        public string IsLock { get; set; }
    }
}
