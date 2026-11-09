using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageDetailGetByTransportPostageIDRequest
    {
        [DataMember]
        public int TransportPostageID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageDetailGetByTransportPostageIDResponse
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
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
        public int ChargingConditionsID { get; set; }
    }
}
