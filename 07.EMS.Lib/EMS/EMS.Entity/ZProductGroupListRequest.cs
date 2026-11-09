using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductGroupListRequest
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZProductGroupListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string EmployeeName { get; set; }
        [DataMember]
        public string ShopName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public int CountProduct { get; set; }
        [DataMember]
        public float TotalPriceProduct { get; set; }
    }
}
