using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageDetailAddRequest
    {
        [DataMember]
        public int PostageID { get; set; }
        [DataMember]
        public string IsStep { get; set; }
        [DataMember]
        public long Weight { get; set; }
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
        public string Key { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
