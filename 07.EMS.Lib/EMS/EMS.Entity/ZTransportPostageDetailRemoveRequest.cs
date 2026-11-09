using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageDetailRemoveRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
