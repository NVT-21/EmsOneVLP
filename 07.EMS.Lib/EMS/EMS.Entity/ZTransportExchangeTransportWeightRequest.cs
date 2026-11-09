using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportExchangeTransportWeightRequest
    {
        [DataMember]
        public int TransportMainServiceID { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
    }
}
