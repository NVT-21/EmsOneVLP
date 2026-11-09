using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ReconciliationListCODMoneyViewModel
    {
        public int ID { get; set; }
        public int ReturnID { get; set; }
        public string CustomerCode { get; set; }
        public string ListCode { get; set; }
        public string EmsBarCode { get; set; }
        public long CODMoney { get; set; }
        public string IsAgree { get; set; }
        public string IsPayment { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string NameStatusReconciliation { get; set; }
        public string CreatedDateOrder { get; set; }
        public string TimeRelease { get; set; }
        public string ApprovalDate { get; set; }
        public string ReasonReject { get; set; }
        public string Period { get; set; }
        public string PeriodName { get; set; }
        public string CODTransTypeName { get; set; }
    }
}