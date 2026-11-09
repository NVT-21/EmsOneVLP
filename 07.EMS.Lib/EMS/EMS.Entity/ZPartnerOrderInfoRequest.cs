using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; } = "";
        [DataMember]
        public string CustomerToken { get; set; } = "";
        [DataMember]
        public string OrderCode { get; set; } = "";
        [DataMember]
        public string ShippingCode { get; set; } = "";
    }


    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public long OrderQuantity { get; set; }
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public ZPartnerOrderInfoShopInfoResponse ShopInfo { get; set; }
        [DataMember]
        public ZPartnerOrderInfoEmployeeInfoResponse EmployeeInfo { get; set; }
        [DataMember]
        public ZPartnerOrderInfoTransportInfoResponse TransportInfo { get; set; }
        [DataMember]
        public ZPartnerOrderInfoSenderInfoResponse SenderInfo { get; set; }
        [DataMember]
        public ZPartnerOrderInfoReceiverInfoResponse ReceiverInfo { get; set; }
        [DataMember]
        public ZPartnerOrderInfoBuyerInfoResponse BuyerInfo { get; set; }
        [DataMember]
        public ZBaseResponse CollectionType { get; set; }
        [DataMember]
        public ZBaseResponse OrderStatus { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoShopInfoResponse
    {
        [DataMember]
        public int ID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoEmployeeInfoResponse
    {
        [DataMember]
        public int ID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoTransportInfoResponse
    {
        [DataMember]
        public ZBaseResponse TransportMainService { get; set; }
        [DataMember]
        public ZBaseResponse TransportExtraService { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long ActualTransportFee { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public double ActualTransportWeight { get; set; }
        [DataMember]
        public double TransportSizeLength { get; set; }
        [DataMember]
        public double TransportSizeWidth { get; set; }
        [DataMember]
        public double TransportSizeHeight { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long ActualTotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoSenderInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoReceiverInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderInfoBuyerInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }
}
