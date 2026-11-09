using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerTransportPostageChargedRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int TransportMainServiceID { get; set; }
        [DataMember]
        public string TransportExtraServiceID { get; set; }
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
        public double TransportWeight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public long OrderQuantity { get; set; }
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerTransportPostageChargedResponse
    {
        [DataMember]
        public long TotalMainFee { get; set; }
        [DataMember]
        public long TotalExtraFee { get; set; }
        [DataMember]
        public long TotalSurcharge { get; set; }
        [DataMember]
        public long TotalAmount { get; set; }

        [DataMember]
        public long TotalMainFeeNotVAT { get; set; }
        [DataMember]
        public long TotalExtraFeeNotVAT { get; set; }
        [DataMember]
        public long TotalSurchargeNotVAT { get; set; }
        [DataMember]
        public long TotalAmountNotVAT { get; set; }

    }
}
