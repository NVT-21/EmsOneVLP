using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZUserGroupSearchListRequest
    {
        [DataMember]
        public string IsLock { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZUserGroupSearchListResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
