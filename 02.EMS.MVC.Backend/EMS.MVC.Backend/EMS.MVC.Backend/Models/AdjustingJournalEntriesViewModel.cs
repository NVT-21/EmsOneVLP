using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class AdjustingJournalEntriesViewModel
    {
        public long SumMainFee { get; set; }
        public long SumServiceFee { get; set; }
        public long SumTransportFeePublic { get; set; }
        public long SumFeePublic { get; set; }
        public long SumProvisionalVolume { get; set; }
        public long SumChargeableVolume { get; set; }
        public List<AdjustingJournalEntriesDataViewModel> ListItem { get; set; }
    }

    public class AdjustingJournalEntriesDataViewModel
    {
        public int AdjustingID { get; set; }
        public string AdjustingPostOffice { get; set; }
        public string AdjustingDate { get; set; }
        public string ItemCode { get; set; }
        public string TimeRelease { get; set; }
        public int ProvisionalVolume { get; set; }
        public int ChargeableVolume { get; set; }
        public long MainFee { get; set; }
        public long ServiceFee { get; set; }
        public long RefundFee { get; set; }
        public long TransportFeePublic { get; set; }
        public long FeePublic { get; set; }
        public string Reason { get; set; }
    }
}