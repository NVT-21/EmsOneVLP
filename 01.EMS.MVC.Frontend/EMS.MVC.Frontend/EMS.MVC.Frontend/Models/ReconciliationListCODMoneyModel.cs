using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ReconciliationListCODMoneyModel
    {
        public int ShopID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        public string ListCode { get; set; }
        public string EmsBarCode { get; set; }
        public string OrderCode { get; set; }
        public string SearchCode { get; set; }
        public int CODTransType { get; set; }
    }
}