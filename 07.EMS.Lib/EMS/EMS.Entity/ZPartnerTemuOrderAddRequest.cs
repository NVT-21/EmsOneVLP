using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerTemuOrderAddRequest
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
        public ZPartnerTemuOrderAddBuyerInfoRequest BuyerInfo { get; set; }
        [DataMember]
        public ZPartnerTemuOrderAddReceiverInfoRequest ReceiverInfo { get; set; }
        [DataMember]
        public ZPartnerTemuOrderAddTransportInfoRequest TransportInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerTemuOrderAddBuyerInfoRequest
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
    public class ZPartnerTemuOrderAddReceiverInfoRequest
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
    public class ZPartnerTemuOrderAddTransportInfoRequest
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
    public class ZPartnerTemuOrderAddResponse
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
