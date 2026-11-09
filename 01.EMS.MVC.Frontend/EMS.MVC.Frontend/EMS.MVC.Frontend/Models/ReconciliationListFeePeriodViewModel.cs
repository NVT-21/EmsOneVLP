using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ReconciliationListFeePeriodViewModel
    {
        public int ID { get; set; }
        public string EmsBarCode { get; set; }
        public string TimeRelease { get; set; }
        public string ReceiveProvice { get; set; }
        public int ProvisionalVolume { get; set; }
        public int ChargeableVolume { get; set; }
        public long ServiceFee { get; set; }
        public long MainFee { get; set; }
        public long ReducedRates { get; set; }
        public long TransportFee { get; set; }
        public string Note { get; set; }
        public string IsAgree { get; set; }
        public string IsPayment { get; set; }
        public string NameStatusReconciliation { get; set; }
        public long TransportFeePublic { get; set; }
        public long FeePublic { get; set; }
        public string ApprovalDate { get; set; }
        public string ReasonReject { get; set; }
        public string Period { get; set; }
        public string PeriodName { get; set; }
    }
}