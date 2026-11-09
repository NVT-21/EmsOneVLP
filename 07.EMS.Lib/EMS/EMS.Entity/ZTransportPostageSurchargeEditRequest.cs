using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageSurchargeEditRequest
    {
        [DataMember]
        public int TransportPostageID { get; set; }
        [DataMember]
        public string SurchargeVAT { get; set; }
        [DataMember]
        public ZTransportPostageSurchargeEditFuelRequest Fuel { get; set; } //Xăng dầu
        [DataMember]
        public ZTransportPostageSurchargeEditRemoteAreaRequest RemoteArea { get; set; } //Vùng xa
        [DataMember]
        public ZTransportPostageSurchargeEditIslandsAreaRequest IslandsArea { get; set; } //Vùng hải đảo
        [DataMember]
        public int UpdateBy { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageSurchargeEditFuelRequest
    {
        [DataMember]
        public string Checked { get; set; }   // Y - N
        [DataMember]
        public int Unit { get; set; }     // 1(VNĐ) - 2(%)
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public double Percent { get; set; }
        [DataMember]
        public long MinValue { get; set; }
        [DataMember]
        public long MaxValue { get; set; }
        [DataMember]
        public string InOfProvince { get; set; }
        [DataMember]
        public string OutOfProvince { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageSurchargeEditRemoteAreaRequest
    {
        [DataMember]
        public string Checked { get; set; }   // Y - N
        [DataMember]
        public int Unit { get; set; }     // 1(VNĐ) - 2(%)
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public double Percent { get; set; }
        [DataMember]
        public long MinValue { get; set; }
        [DataMember]
        public long MaxValue { get; set; }
        [DataMember]
        public string InOfProvince { get; set; }
        [DataMember]
        public string OutOfProvince { get; set; }
        [DataMember]
        public string IsFrom { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageSurchargeEditIslandsAreaRequest
    {
        [DataMember]
        public string Checked { get; set; }   // Y - N
        [DataMember]
        public int Unit { get; set; }     // 1(VNĐ) - 2(%)
        [DataMember]
        public double Value { get; set; }
        [DataMember]
        public double Percent { get; set; }
        [DataMember]
        public long MinValue { get; set; }
        [DataMember]
        public long MaxValue { get; set; }
        [DataMember]
        public string InOfProvince { get; set; }
        [DataMember]
        public string OutOfProvince { get; set; }
        [DataMember]
        public string IsFrom { get; set; }
    }
}
