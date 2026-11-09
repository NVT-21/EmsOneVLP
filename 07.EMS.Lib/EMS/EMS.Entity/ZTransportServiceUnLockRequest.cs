using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportServiceUnLockRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
