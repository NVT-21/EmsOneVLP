using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageDetailEditRequest
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public int LocalFee { get; set; }
        [DataMember]
        public int IntercityFee1 { get; set; }
        [DataMember]
        public int IntercityFee2 { get; set; }
        [DataMember]
        public int IntercityFee3 { get; set; }
        [DataMember]
        public int IntercityFee4 { get; set; }
        [DataMember]
        public int IntercityFee5 { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
    }
}
