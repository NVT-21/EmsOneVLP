using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    public class ReconciliationListFeeViewModel
    {
        public string EmsBarCode { get; set; }
        public int OrderID { get; set; }
        public string OrderCode { get; set; }
        public string ReceiveProvice { get; set; }
        public string CreatedDateOrder { get; set; }
        public string TimeRelease { get; set; }
        public long ServiceFee { get; set; }
        public long MainFee { get; set; }
        public long ReducedRates { get; set; }
        public long TempTransportFee { get; set; }
        public long TransportFee { get; set; }
        public long TransportFeePublic { get; set; }
        public long FeePublic { get; set; }
        public int ProvisionalVolume { get; set; }
        public int ChargeableVolume { get; set; }
        public string ServiceCode { get; set; }
        public string AdditionService { get; set; }
        public string SpecialService { get; set; }
        public string ListCode { get; set; }
        public string TypeItemName { get; set; }
    }
}