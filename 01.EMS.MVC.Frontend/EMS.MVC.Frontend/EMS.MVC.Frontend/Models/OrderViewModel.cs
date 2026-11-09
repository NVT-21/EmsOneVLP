using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Entity;
namespace EMS.MVC.Frontend.Models
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public int ShopID { get; set; }
        public string AllDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public List<ZShopGetListCrmOrPaypostCodeResponse> ListCrmOrPaypostCode { get; set; }
        public string OrderStatus { get; set; }
        public List<SelectListItem> ListOrderStatus { get; set; }
        public string PrintStatus { get; set; }
        public List<SelectListItem> ListPrintStatus { get; set; }
        public int PostalType { get; set; }
        public List<SelectListItem> ListPostalType { get; set; }
        public string MainTransportService { get; set; }
        public List<TransportServiceModel> ListMainTransportService { get; set; }
        public string ExtraTransportService { get; set; }
        public List<TransportServiceModel> ListExtraTransportService { get; set; }
        public string SearchKey { get; set; } //Mã đơn hàng, mã bưu gửi, sđt người nhận
        //public string ReloadCountStatus { get; set; }
        public string ReloadStatusNumber { get; set; }


        public int PageNumber { get; set; } = 1;
        public int PageLimit { get; set; } = 20;
    }
}