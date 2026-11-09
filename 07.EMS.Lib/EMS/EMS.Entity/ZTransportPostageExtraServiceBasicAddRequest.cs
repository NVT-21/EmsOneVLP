using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageExtraServiceBasicAddRequest
    {
        [DataMember]
        public int ExtraTransportServiceID { get; set; }
        [DataMember]
        public string AppliesToShop { get; set; }
        [DataMember]
        public int SubjectOfApplication { get; set; }
        [DataMember]
        public string VAT { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int ChargingConditionsID { get; set; }
        [DataMember]
        public double ChargingConditionsValue { get; set; }
        [DataMember]
        public long ChargingConditionsMinValue { get; set; }
        [DataMember]
        public string ServicesUsed { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
    }
}
