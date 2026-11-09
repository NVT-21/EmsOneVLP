using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Areas.Tiktok.Models
{
    public class TIK_OrderViewModel
    {
        public string OrderID { get; set; }
        public int ShopID { get; set; }
        public string AllDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OrderStatus { get; set; }
        public List<SelectListItem> ListOrderStatus { get; set; }
        public string SearchKey { get; set; }
        public string ReloadStatusNumber { get; set; }
        public string PaymentMethod { get; set; }
        public List<SelectListItem> ListPaymentMethod { get; set; }
    }
}