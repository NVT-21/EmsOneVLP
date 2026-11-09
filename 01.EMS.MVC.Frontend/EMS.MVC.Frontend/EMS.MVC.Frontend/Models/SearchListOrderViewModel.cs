using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class SearchListOrderViewModel
    {
        public int OrderID { get; set; }
        public string EMSOneCode { get; set; }
        public string OrderCode { get; set; }
        public string BatchCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerFullAddress { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        //public int SenderProvinceID { get; set; }
        //public int SenderDistrictID { get; set; }
        //public int SenderWardID { get; set; }
        //public string SenderAddress { get; set; }
        public string SenderFullAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public int ReceiverProvinceID { get; set; }
        //public string ReceiverProvinceName { get; set; }
        public int ReceiverDistrictID { get; set; }
        //public string ReceiverDistrictName { get; set; }
        public int ReceiverWardID { get; set; }
        //public string ReceiverWardName { get; set; }
        public string ReceiverStreet { get; set; }
        public string ReceiverFullAddress { get; set; }
        public string Message { get; set; }
        public long TotalCOD { get; set; }
        //public long TotalOrderValue { get; set; }
        //public long TotalProductValue { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusName { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedName { get; set; }
        public string UpdatedDate { get; set; }
        public string ReasonCancel { get; set; }
        public string ShippingCode { get; set; }
        //public int InventoryID { get; set; }
        //public string Transport { get; set; }
        public string TransportOrderName { get; set; }
        public long TransportFee { get; set; }
        //public string TrackingItemInfoCreatedDate { get; set; }
        //public string OrderHistoryDescription { get; set; }
        public int ShopID { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public long PaymentFreightVat { get; set; }
        public string PrintStatus { get; set; }
        public string TransportMainServiceCode { get; set; }
        public string TransportMainServiceName { get; set; }
        public string TransportExtraServiceCode { get; set; }
        public string TransportExtraServiceName { get; set; }
        public string SearchKey { get; set; }
    }
}