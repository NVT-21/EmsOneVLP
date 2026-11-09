using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZPartnerOrderPrintRequest
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; } = "";
        [DataMember]
        public string CustomerToken { get; set; } = "";
        [DataMember]
        public string OrderCode { get; set; } = "";
        [DataMember]
        public string ShippingCode { get; set; } = "";
        [DataMember]
        public string PrintType { get; set; } = "";
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderPrint_V2Request
    {
        [DataMember]
        public string CrmOrPaypostCode { get; set; } = "";
        [DataMember]
        public string OrderCode { get; set; } = "";
        [DataMember]
        public string ShippingCode { get; set; } = "";
        [DataMember]
        public string PosCode { get; set; } = "";
        [DataMember]
        public string SendingTime { get; set; } = "";
        [DataMember]
        public string SenderName { get; set; } = "";
        [DataMember]
        public string ReceiverName { get; set; } = "";
        [DataMember]
        public string ReceiverPhone { get; set; } = "";
        [DataMember]
        public string ProductDescription { get; set; } = "";
        [DataMember]
        public string Weight { get; set; } = "";
        [DataMember]
        public string CodAmount { get; set; } = "";
        [DataMember]
        public string DeliveryFee { get; set; } = "";
        [DataMember]
        public string Service { get; set; } = "";
        [DataMember]
        public string Note { get; set; } = "";
        [DataMember]
        public string PrintTime { get; set; } = "";
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderPrint_V2Response
    {
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string Label { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderPrintResponse
    {
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string BatchCode { get; set; }
        [DataMember]
        public byte[] OrderBarCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public byte[] ShippingBarCode { get; set; }
        [DataMember]
        public byte[] QRCode { get; set; }
        [DataMember]
        public long OrderQuantity { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public ZPartnerOrderPrintTransportInfoResponse TransportInfo { get; set; }
        [DataMember]
        public ZPartnerOrderPrintSenderInfoResponse SenderInfo { get; set; }
        [DataMember]
        public ZPartnerOrderPrintReceiverInfoResponse ReceiverInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderPrintTransportInfoResponse
    {
        [DataMember]
        public string PickUpProvinceName { get; set; }
        [DataMember]
        public ZBaseResponse TransportMainService { get; set; }
        [DataMember]
        public ZBaseResponse TransportExtraService { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public long PaymentFreightVat { get; set; }
        [DataMember]
        public ZBaseResponse TransportPayer { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public string PostOffice { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderPrintSenderInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string SentDate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZPartnerOrderPrintReceiverInfoResponse
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string ProvinceName { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
    }
}
