using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class PaymentListCODViewModel
    {
        public int STT { get; set; }
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPPCRM { get; set; }
        public int OrderID { get; set; }
        public int ShopID { get; set; }
        public string OrderCode { get; set; }
        public string EmsBarCode { get; set; }
        public long CODMoney { get; set; }
        public string IsAgree { get; set; }
        public string StatusIsAgree { get; set; }
        public string IsPayment { get; set; }
        public string StatusIsPayment { get; set; }
    }
}