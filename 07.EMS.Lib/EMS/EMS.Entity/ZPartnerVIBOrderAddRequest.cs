using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerVIBOrderAddRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long OrderQuantity { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public ZPartnerVIBOrderAddBuyerInfoRequest BuyerInfo { get; set; }
        [DataMember]
        public ZPartnerVIBOrderAddReceiverInfoRequest ReceiverInfo { get; set; }
        [DataMember]
        public ZPartnerVIBOrderAddTransportInfoRequest TransportInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerVIBOrderAddBuyerInfoRequest
    {
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerVIBOrderAddReceiverInfoRequest
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public int ProvinceID { get; set; }
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int WardID { get; set; }
        [DataMember]
        public string Street { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerVIBOrderAddTransportInfoRequest
    {
        [DataMember]
        public int TransportPayer { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public int TransportMainServiceID { get; set; }
        [DataMember]
        public string TransportExtraServiceID { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public int CollectionType { get; set; }
        //[DataMember]
        //public long TotalCOD { get; set; }
        //[DataMember]
        //public long TotalPHU { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerVIBOrderAddResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public int TransportFee { get; set; }

    }
}
