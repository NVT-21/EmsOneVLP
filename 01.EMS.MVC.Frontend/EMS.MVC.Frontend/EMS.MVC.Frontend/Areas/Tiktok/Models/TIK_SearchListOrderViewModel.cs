using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Areas.Tiktok.Models
{
    [Serializable]
    [DataContract]
    public class TIK_SearchListOrderViewModel
    {
        [DataMember]
        public int OrderID { get; set; }
        [DataMember]
        public string EMSOneCode { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerPhone { get; set; }
        [DataMember]
        public string CustomerFullAddress { get; set; }
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
        public long TotalOrderValue { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public string OrderStatusName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string UpdatedName { get; set; }
        [DataMember]
        public string ShippingCode { get; set; }
        [DataMember]
        public string TransportOrderName { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public int ShopID { get; set; }
        [DataMember]
        public string CrmOrPaypostCode { get; set; }
        [DataMember]
        public long PaymentFreightVat { get; set; }
        [DataMember]
        public string PrintStatus { get; set; }
        [DataMember]
        public string TransportMainServiceCode { get; set; }
        [DataMember]
        public string TransportMainServiceName { get; set; }
        [DataMember]
        public string TransportExtraServiceCode { get; set; }
        [DataMember]
        public string TransportExtraServiceName { get; set; }
        [DataMember]
        public string SearchKey { get; set; }
        [DataMember]
        public string TiktokReverseOrderCode { get; set; }
        [DataMember]
        public XStatusModel TiktokOrderInventoryStatus { get; set; }
    }
}