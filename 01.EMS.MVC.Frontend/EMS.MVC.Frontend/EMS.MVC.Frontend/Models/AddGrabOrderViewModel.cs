using EMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class AddGrabOrderViewModel
    {
        [DataMember]
        public string Channel { get; set; }
        [DataMember]
        public List<SelectListItem> ListChannel { get; set; }
        [DataMember]
        public int WareHouseID { get; set; }
        [DataMember]
        public List<WareHouseModel> ListWareHouse { get; set; }
        [DataMember]
        public int MainTransportServiceID { get; set; }
        [DataMember]
        public string MainTransportServiceCode { get; set; }
        [DataMember]
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        [DataMember]
        public string ExtraTransportServiceID { get; set; }
        [DataMember]
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        [DataMember]
        public int TransportPayer { get; set; }
        [DataMember]
        public XBaseModel CollectionType { get; set; }
        [DataMember]
        public CustomerModel Buyer { get; set; }
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string IsTransport { get; set; }
        [DataMember]
        public string OrderCode { get; set; }
        [DataMember]
        public string OrderName { get; set; }
        [DataMember]
        public double Weight { get; set; }
        [DataMember]
        public long Quantity { get; set; }
        [DataMember]
        public double Length { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Note { get; set; }
        [DataMember]
        public int VariantID { get; set; }
        [DataMember]
        public int NumberOfProduct { get; set; }
        [DataMember]
        public CustomerModel Receiver { get; set; }
        [DataMember]
        public string IsUpdateReceiver { get; set; }
        [DataMember]
        public CustomerModel Sender { get; set; }
        [DataMember]
        public int RequestOrderID { get; set; }
        [DataMember]
        public string IsRequestOrder { get; set; }
        [DataMember]
        public int AddType { get; set; }
        [DataMember]
        public long OrderValue { get; set; }
        [DataMember]
        public long TotalMoney { get; set; }
        [DataMember]
        public long TotalDiscount { get; set; }
        [DataMember]
        public long TotalOrderValue { get; set; }
        [DataMember]
        public long TotalCOD { get; set; }
        [DataMember]
        public long TotalPHU { get; set; }
        [DataMember]
        public long TransportFee { get; set; }
        [DataMember]
        public List<ZOrderAddItemRequest> ListProduct { get; set; }
        [DataMember]
        public List<ZInventoryDetailAddRequest> ListInventoryOutput { get; set; }
        [DataMember]
        public string OrderStatus { get; set; }
        [DataMember]
        public int PickUpAddressID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public string PromoCode { get; set; }
        [DataMember]
        public string PaymentMethod { get; set; }
    }
}