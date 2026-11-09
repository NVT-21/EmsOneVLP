using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageUnConfirmRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
