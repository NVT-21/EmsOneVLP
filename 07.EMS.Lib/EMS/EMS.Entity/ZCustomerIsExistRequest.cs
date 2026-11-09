using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCustomerIsExistRequest
    {
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public string IsUpdated { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
    }
}
