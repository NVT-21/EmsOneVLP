using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Entity
{
    public class ZReconciliationSearchListCODRequest
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
    public class ZReconciliationSearchListCODResponse
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
        public string CreatedDateOrder { get; set; }
        public string TimeRelease { get; set; }
        public string ApprovalDate { get; set; }
        public string ReasonReject { get; set; }
        public string PeriodName { get; set; }
        public string CODTransTypeName { get; set; }
    }
    public class ZReconciliationUpdateAgreeCODRequest
    {
        public int ID { get; set; }
        public string Agree { get; set; }
        public string ReasonReject { get; set; }
        public int UpdatedBy { get; set; }
    }
}
