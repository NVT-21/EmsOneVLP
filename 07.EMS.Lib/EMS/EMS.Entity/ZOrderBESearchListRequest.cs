using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderBESearchListRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string ShopCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public int ReceiverProvinceID { get; set; }
        [DataMember]
        public int TransportMainService { get; set; }
        [DataMember]
        public string TransportExtraService { get; set; }
        [DataMember]
        public string PlaceOfCreateOrder { get; set; }
        [DataMember]
        public int PostalType { get; set; }
        [DataMember]
        public int PartnerID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        //[DataMember]
        //public int PageNumber { get; set; }
        //[DataMember]
        //public int PageSize { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderBESearchListResponse
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; } // Mã EMSOne
        [DataMember]
        public string OrderCode { get; set; } // Mã đơn hàng
        [DataMember]
        public string BatchCode { get; set; } // Mã lô

        [DataMember]
        public string SenderName { get; set; }
        [DataMember]
        public string SenderPhone { get; set; }
        [DataMember]
        public string SenderFullAddress { get; set; }

        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string ReceiverPhone { get; set; }
        [DataMember]
        public string ReceiverFullAddress { get; set; }

        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public string Transport { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public double ProvisionalTransportWeight { get; set; }
        [DataMember]
        public double ActualTransportWeight { get; set; }
        [DataMember]
        public ZBaseResponse OrderStatus { get; set; }
        [DataMember]
        public ZBaseResponse TransportMainService { get; set; }
        [DataMember]
        public ZBaseResponse TransportExtraService { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public string ReasonCancel { get; set; }


        [DataMember]
        public string PostalType { get; set; }


        [DataMember]
        public string ShippingID { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }

        [DataMember]
        public long PaymentFreightVat { get; set; }
        [DataMember]
        public string TrackingItemInfoCreatedDate { get; set; }
        [DataMember]
        public string AcceptancePosCode { get; set; }
        [DataMember]
        public string DeliveryPosCode { get; set; }


        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string ShopCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public string PrintTypeDefault { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string PartnerName { get; set; }

        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerPhone { get; set; }
        [DataMember]
        public string CustomerFullAddress { get; set; }
    }
}
