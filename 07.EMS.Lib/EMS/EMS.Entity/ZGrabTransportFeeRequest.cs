using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZGrabTransportFeeRequest
    {
        [DataMember]
        public int MainServiceID { get; set; }

        [DataMember]
        public int FromProvinceID { get; set; }
        [DataMember]
        public int FromDistrictID { get; set; }
        [DataMember]
        public int FromWardID { get; set; }
        [DataMember]
        public string FromFullAddress { get; set; }

        [DataMember]
        public int ToProvinceID { get; set; }
        [DataMember]
        public int ToDistrictID { get; set; }
        [DataMember]
        public int ToWardID { get; set; }
        [DataMember]
        public string ToFullAddress { get; set; }

        [DataMember]
        public int Weight { get; set; }
        [DataMember]
        public int Length { get; set; }
        [DataMember]
        public int Width { get; set; }
        [DataMember]
        public int Height { get; set; }
        [DataMember]
        public int COD { get; set; }
        [DataMember]
        public int Amount { get; set; }
        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string PromoCode { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZGrabTransportFeeResponse
    {
        [DataMember]
        public double Distance { get; set; }
        [DataMember]
        public int TotalFreight { get; set; }
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string EstimatedTimeLinePickup { get; set; }
        [DataMember]
        public string EstimatedTimeLineDropoff { get; set; }
        [DataMember]
        public string EstimatedTimeLineCompleted { get; set; }
    }
}
