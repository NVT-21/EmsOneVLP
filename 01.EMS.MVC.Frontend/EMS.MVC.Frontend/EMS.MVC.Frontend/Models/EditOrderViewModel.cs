using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Entity;
using System.Web.Mvc;
namespace EMS.MVC.Frontend.Models
{
    public class EditOrderViewModel
    {
        public CustomerModel Buyer { get; set; }
        public CustomerModel Receiver { get; set; }
        public string IsUpdateReceiver { get; set; }
        public CustomerModel Sender { get; set; }
        public int WareHouseID { get; set; }
        public List<WareHouseModel> ListWareHouse { get; set; }
        public List<SearchListProductViewModel> ListProduct { get; set; }
        public List<ZInventoryDetailAddRequest> ListInventoryOutput { get; set; }
        public string IsTransport { get; set; }
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public long OrderValue { get; set; }
        public long Quantity { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public BaseModel CollectionType { get; set; }
        public int TransportPayer { get; set; }
        public int MainTransportServiceID { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportServiceID { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public long TransportFee { get; set; }
        public long TotalCOD { get; set; }
        public long TotalPHU { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        [AllowHtml]
        public string Note { get; set; }
        public string Channel { get; set; }
        public List<SelectListItem> ListChannel { get; set; }
        public long TotalMoney { get; set; }
        public long TotalDiscount { get; set; }
        public long TotalOrderValue { get; set; }
        public string OrderStatus { get; set; }
        public int AddType { get; set; }
        public string EMSOneCode { get; set; }
        public string CreatedDate { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public int CollectionPostOfficeRecentID { get; set; }
        //public int DeliveryNoteID { get; set; }
        //public string DeliveryNoteCode { get; set; }
    }
}