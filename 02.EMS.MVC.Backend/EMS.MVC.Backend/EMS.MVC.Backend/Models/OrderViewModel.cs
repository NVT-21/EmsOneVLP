using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    public class OrderViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OrderCode { get; set; }
        public string ShippingCode { get; set; }
        public string ShopCode { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string OrderStatus { get; set; }
        public List<SelectListItem> ListOrderStatus { get; set; }
        public int ReceiverProvinceID { get; set; }
        public List<ProvinceModel> ListProvince { get; set; }
        public int TransportMainService { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string TransportExtraService { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public string PlaceOfCreateOrder { get; set; }
        public List<SelectListItem> ListPlaceOfCreateOrder { get; set; }
        public int PostalType { get; set; }
        public List<SelectListItem> ListPostalType { get; set; }
        public string ReloadStatusNumber { get; set; }
        public string TypeOrder { get; set; }
        public List<SelectListItem> ListTypeOrder { get; set; }
        public int PartnerID { get; set; }
        public List<PartnerModel> ListPartner { get; set; }
    }
}