using System;
using System.Runtime.Serialization;

namespace EMS.Entity
{
    [Serializable]
    [DataContract]
    public class ZOrderFESearchListRequest
    {
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string ListCustomerCode { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string PrintStatus { get; set; }
        [DataMember]
        public int PostalType { get; set; }
        [DataMember]
        public string MainTransportService { get; set; }
        [DataMember]
        public string ExtraTransportService { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string Channel { get; set; }
        //[DataMember]
        //public int PageNumber { get; set; }
        //[DataMember]
        //public int PageSize { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ZOrderFESearchListResponse
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
        public int SenderProvinceID { get; set; }
        [DataMember]
        public int SenderDistrictID { get; set; }
        [DataMember]
        public int SenderWardID { get; set; }
        [DataMember]
        public string SenderAddress { get; set; }
        [DataMember]
        public string SenderFullAddress { get; set; }
        [DataMember]
        public string ReceiverName { get; set; }
        [DataMember]
        public string ReceiverPhone { get; set; }
        [DataMember]
        public int ReceiverProvinceID { get; set; }
        [DataMember]
        public string ReceiverProvinceName { get; set; }
        [DataMember]
        public int ReceiverDistrictID { get; set; }
        [DataMember]
        public string ReceiverDistrictName { get; set; }
        [DataMember]
        public int ReceiverWardID { get; set; }
        [DataMember]
        public string ReceiverWardName { get; set; }
        [DataMember]
        public string ReceiverAddress { get; set; }
        [DataMember]
        public string ReceiverFullAddress { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalOrderValue { get; set; }
        [DataMember]
        public long TotalProductValue { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string OrderStatusName { get; set; }
        [DataMember]
        public string CreatedOrderDate { get; set; }
        [DataMember]
        public string AcceptOrderDate { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string Transport { get; set; }
        [DataMember]
        public string TransportOrderName { get; set; }
        [DataMember]
        public long TransportOrderValue { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public double TransportWeight { get; set; }
        [DataMember]
        public string TransportMainServiceCode { get; set; }
        [DataMember]
        public string TransportMainServiceName { get; set; }
        [DataMember]
        public string TransportExtraServiceCode { get; set; }
        [DataMember]
        public string TransportExtraServiceName { get; set; }
        [DataMember]
        public string PrintStatus { get; set; }
        [DataMember]
        public ZBaseResponse Channel { get; set; }

        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string ShopCode { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }

        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerPhone { get; set; }
        [DataMember]
        public string CustomerFullAddress { get; set; }

        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string UpdatedDate { get; set; }
        [DataMember]
        public string ReasonCancel { get; set; }

        [DataMember]
        public string ShippingID { get; set; }

        [DataMember]
        public string ShippingCode { get; set; }
        
        [DataMember]
        public long PaymentFreightVat { get; set; }

        [DataMember]
        public string TiktokReverseOrderCode { get; set; }

        [DataMember]
        public XStatusModel TiktokOrderInventoryStatus { get; set; }
        [DataMember]
        public string CreateBy { get; set; }
    }
}
