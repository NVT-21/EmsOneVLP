using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageGetByTransportPostageIDRequest
    {
        [DataMember]
        public int TransportPostageID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageGetByTransportPostageIDResponse
    {
        [DataMember]
        public int TransportPostageID { get; set; }
        [DataMember]
        public string TransportPostageCode { get; set; }
        [DataMember]
        public string TransportPostageName { get; set; }
        //[DataMember]
        //public int TransportPostageType { get; set; }
        //[DataMember]
        //public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int MainTransportServiceID { get; set; }
        [DataMember]
        public string ExtraTransportServiceID { get; set; }
        [DataMember]
        public int Area { get; set; }
        [DataMember]
        public int SubjectOfApplication { get; set; }
        //[DataMember]
        //public string ApplicationObjectValue { get; set; }
        [DataMember]
        public string AppliesToShop { get; set; }
        [DataMember]
        public string AppliesToProvinceFrom { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int ProvinceApply { get; set; }
        [DataMember]
        public int AreaApply { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }


        [DataMember]
        public int MainFeeRange { get; set; }
        [DataMember]
        public string MainFeeVAT { get; set; }
        [DataMember]
        public int UpdateBy { get; set; }
        [DataMember]
        public string UpdateDate { get; set; }

        [DataMember]
        public string SurchargeVAT { get; set; }

        [DataMember]
        public ZTransportPostageGetByTransportPostageIDFuelResponse Fuel { get; set; } //Xăng dầu
        [DataMember]
        public ZTransportPostageGetByTransportPostageIDRemoteAreaResponse RemoteArea { get; set; } //Vùng xa
        [DataMember]
        public ZTransportPostageGetByTransportPostageIDIslandsAreaResponse IslandsArea { get; set; } //Vùng hải đảo

        [DataMember]
        public string TransportServiceName { get; set; }
        [DataMember]
        public ZBaseResponse VAT { get; set; }
        [DataMember]
        public int ChargingConditionsID { get; set; }
        [DataMember]
        public double ChargingConditionsValue { get; set; }
        [DataMember]
        public long ChargingConditionsMinValue { get; set; }
        [DataMember]
        public string ServicesUsed { get; set; }

    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageGetByTransportPostageIDFuelResponse
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
    public class ZTransportPostageGetByTransportPostageIDRemoteAreaResponse
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
    public class ZTransportPostageGetByTransportPostageIDIslandsAreaResponse
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
