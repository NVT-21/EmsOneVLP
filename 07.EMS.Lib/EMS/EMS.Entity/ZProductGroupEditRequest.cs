using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductGroupEditRequest
    {
        [DataMember]
        public int ProductGroupID { get; set; }
        [DataMember]
        public string ProductGroupName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
    public class ZProductGroupEditResponse
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
