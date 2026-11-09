using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    public class WalkInOrderViewModel
    {
        public string Channel { get; set; }
        public List<SelectListItem> ListChannel { get; set; }
        public int MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public int TransportPayer { get; set; }
        public BaseModel CollectionType { get; set; }
        public CustomerModel Buyer { get; set; }
        public string MobileNumber { get; set; }
        public string IsTransport { get; set; }
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public double Weight { get; set; }
        public long Quantity { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        [AllowHtml]
        public string Note { get; set; }
        public CustomerModel Receiver { get; set; }
        public CustomerModel Sender { get; set; }
        public long OrderValue { get; set; }
        public long TotalMoney { get; set; }
        public long TotalDiscount { get; set; }
        public long TotalOrderValue { get; set; }
        public long TotalCOD { get; set; }
        public long TotalPHU { get; set; }
        public long TransportFee { get; set; }
        public string IsHiddenProduct { get; set; } = "N";
        public string OrderStatus { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public int CollectionPostOfficeRecentID { get; set; }
        public int TypeShop { get; set; }
        public int ReturnCount { get; set; }
        public string ReturnShippingCodes { get; set; }
    }
}