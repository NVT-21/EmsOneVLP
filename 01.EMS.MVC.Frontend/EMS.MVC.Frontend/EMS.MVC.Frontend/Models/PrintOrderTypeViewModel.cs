using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class PrintOrderTypeViewModel
    {
        public int Page { get; set; }
        public List<PrintOrderTypeDataModel> ListOrderPrint { get; set; }
    }
    public class PrintOrderTypeDataModel
    {
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string BatchCode { get; set; }
        public string ShippingCode { get; set; }
        public string PickUpProvinceName { get; set; }
        public string SenderFullName { get; set; }
        public string SenderFullAddress { get; set; }
        public string SenderDistrictName { get; set; }
        public string SenderProvinceName { get; set; }
        public string SenderMobileNumber { get; set; }
        public string SenderSentDate { get; set; }
        public string RecieverFullName { get; set; }
        public string RecieverFullAddress { get; set; }
        public string RecieverDistrictName { get; set; }
        public string RecieverProvinceName { get; set; }
        public string RecieverMobileNumber { get; set; }
        public string Message { get; set; }
        public string Note { get; set; }
        public string TransportWeight { get; set; }
        public string TransportQuantity { get; set; }
        public string TransportMainServiceCode { get; set; }
        public string TransportMainServiceName { get; set; }
        public string TransportExtraServiceCode { get; set; }
        public string TransportExtraServiceName { get; set; }
        public long TotalCOD { get; set; }
        public long TotalPHU { get; set; }
        public long PaymentFreightVat { get; set; }
        public int TransportPayer { get; set; }
        public long TransportFee { get; set; }
        public byte[] QRCode { get; set; }
        public byte[] ShippingBarCode { get; set; }
        public byte[] OrderBarCode { get; set; }
        public int ShopID { get; set; }
        public string PostOffice { get; set; }
    }
}