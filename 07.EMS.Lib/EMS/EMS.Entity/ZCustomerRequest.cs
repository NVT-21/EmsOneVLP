using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZCustomerRequest
    {
        
    }

    [Serializable]
    [DataContract]
    public class ZCustomerResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string BlackList { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public int SuccessOrder { get; set; }
        [DataMember]
        public int TotalOrder { get; set; }
    }
}
