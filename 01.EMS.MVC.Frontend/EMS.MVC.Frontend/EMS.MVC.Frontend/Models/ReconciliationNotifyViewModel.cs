using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ReconciliationNotifyViewModel
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string TypeText { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CreatedDate { get; set; }
        public string NotifyDate { get; set; }
        public string NotifyMessage { get; set; }
        public int ShopID { get; set; }
        public string IsCheck { get; set; }
    }
}