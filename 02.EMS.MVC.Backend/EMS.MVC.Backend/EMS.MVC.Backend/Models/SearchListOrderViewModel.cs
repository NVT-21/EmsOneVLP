using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class SearchListOrderViewModel
    {
        public int OrderID { get; set; }
        public string EMSOneCode { get; set; }
        public string OrderCode { get; set; }
        public string BatchCode { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderFullAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceiverFullAddress { get; set; }
        public string Message { get; set; }
        public long TotalCOD { get; set; }
        public string Transport { get; set; }
        public long TransportFee { get; set; }
        public double ProvisionalTransportWeight { get; set; }
        public double ActualTransportWeight { get; set; }
        public BaseModel OrderStatus { get; set; }
        public BaseModel TransportMainService { get; set; }
        public BaseModel TransportExtraService { get; set; }
        public string CreatedDate { get; set; }

        public string UpdatedName { get; set; }
        public string UpdatedDate { get; set; }
        public string ReasonCancel { get; set; }

        public string PostalType { get; set; }

        public string ShippingID { get; set; }
        public string ShippingCode { get; set; }

        public long PaymentFreightVat { get; set; }
        public string TrackingItemInfoCreatedDate { get; set; }

        public int ShopID { get; set; }
        public string ShopCode { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string PrintTypeDefault { get; set; }
        public string StoreName { get; set; }
        public string PartnerName { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerFullAddress { get; set; }
    }
}