using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductGroupAddRequest
    {
        [DataMember]
        public string ProductGroupCode { get; set; }
        [DataMember]
        public string ProductGroupName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
