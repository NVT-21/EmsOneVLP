using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZTransportPostageChargedRequest
    {
        [DataMember]
        public int MainServiceID { get; set; }
        [DataMember]
        public string ExtraServiceIDs { get; set; }
        [DataMember]
        public int FromProvinceID { get; set; }
        [DataMember]
        public int FromDistrictID { get; set; }
        [DataMember]
        public int FromWardID { get; set; }
        [DataMember]
        public int ToProvinceID { get; set; }
        [DataMember]
        public int ToDistrictID { get; set; }
        [DataMember]
        public int ToWardID { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public long Quantity { get; set; }
        [DataMember]
        public long Amount { get; set; }
        [DataMember]
        public long COD { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageChargedResponse
    {
        [DataMember]
        public long MainFee { get; set; }
        [DataMember]
        public long ExtraFee { get; set; }
        [DataMember]
        public long Surcharge { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }

        [DataMember]
        public long MainFeeNotVAT { get; set; }
        [DataMember]
        public long ExtraFeeNotVAT { get; set; }
        [DataMember]
        public long SurchargeNotVAT { get; set; }
        [DataMember]
        public long TotalAmountNotVAT { get; set; }
        [DataMember]
        public List<ZTransportPostageExtraChargedResponse> TransportPostageExtraChargedDetail { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageExtraChargedResponse
    {
        [DataMember]
        public string TransportExtraService { get; set; }
        [DataMember]
        public long Fee { get; set; }
        
    }

    [Serializable]
    [DataContract]
    public class ZTransportPostageExtraChargedEEResponse
    {
        [DataMember]
        public string DVCT { get; set; }
        [DataMember]
        public string CUOC { get; set; }

    }

}
