using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZProductGroupGetByIDRequest
    {
        [DataMember]
        public int ProductGroupID { get; set; }
        [DataMember]
        public int ShopID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZProductGroupGetByIDResponse
    {
        [DataMember]
        public int ProductGroupID { get; set; }
        [DataMember]
        public string ProductGroupCode { get; set; }
        [DataMember]
        public string ProductGroupName { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
