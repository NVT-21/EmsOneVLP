using System;
using System.Runtime.Serialization;
namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageChangeIsLockRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string IsLock { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        
    }
}
