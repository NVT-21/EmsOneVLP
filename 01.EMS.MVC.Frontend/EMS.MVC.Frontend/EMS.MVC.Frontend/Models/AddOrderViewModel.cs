using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class AddOrderViewModel
    {
        public string Channel { get; set; }
        public List<SelectListItem> ListChannel { get; set; }
        //public string PaymentMethods { get; set; }
        //public List<SelectListItem> ListPaymentMethods { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public int MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        //public string PaymentStatus { get; set; }
        //public List<SelectListItem> ListPaymentStatus { get; set; }
        public int TransportPayer { get; set; }
        public XBaseModel CollectionType { get; set; }
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
        public int VariantID { get; set; }
        public int NumberOfProduct { get; set; }
        public CustomerModel Receiver { get; set; }
        public string IsUpdateReceiver { get; set; }
        public CustomerModel Sender { get; set; }
        public int RequestOrderID { get; set; }
        public string IsRequestOrder { get; set; }
        public int AddType { get; set; }
        public long OrderValue { get; set; }
        public long TotalMoney { get; set; }
        public long TotalDiscount { get; set; }
        public long TotalOrderValue { get; set; }
        public long TotalCOD { get; set; }
        public long TotalPHU { get; set; }
        public long TransportFee { get; set; }
        public List<ZOrderAddItemRequest> ListProduct { get; set; }
        public List<ZInventoryDetailAddRequest> ListInventoryOutput { get; set; }
        public string ShowView { get; set; } = "AddOrderView";
        public string IsLoad { get; set; }
        public string OrderStatus { get; set; }
        public int PickUpAddressID { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public int CollectionPostOfficeRecentID { get; set; }
        public string CrmOrPayPostCode { get; set; }
    }
}