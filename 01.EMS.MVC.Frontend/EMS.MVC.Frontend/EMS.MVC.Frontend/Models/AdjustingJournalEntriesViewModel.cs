using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class AdjustingJournalEntriesViewModel
    {
        [DataMember]
        public long SumMainFee { get; set; }
        [DataMember]
        public long SumServiceFee { get; set; }
        [DataMember]
        public long SumTransportFeePublic { get; set; }
        [DataMember]
        public long SumFeePublic { get; set; }
        [DataMember]
        public long SumProvisionalVolume { get; set; }
        [DataMember]
        public long SumChargeableVolume { get; set; }
        [DataMember]
        public List<AdjustingJournalEntriesDataViewModel> ListItem { get; set; }
    }
    [Serializable]
    [DataContract]
    public class AdjustingJournalEntriesDataViewModel
    {
        [DataMember]
        public int AdjustingID { get; set; }
        [DataMember]
        public string AdjustingPostOffice { get; set; }
        [DataMember]
        public string AdjustingDate { get; set; }
        [DataMember]
        public string ItemCode { get; set; }
        [DataMember]
        public string TimeRelease { get; set; }
        [DataMember]
        public int ProvisionalVolume { get; set; }
        [DataMember]
        public int ChargeableVolume { get; set; }
        [DataMember]
        public long MainFee { get; set; }
        [DataMember]
        public long ServiceFee { get; set; }
        [DataMember]
        public long RefundFee { get; set; }
        [DataMember]
        public long TransportFeePublic { get; set; }
        [DataMember]
        public long FeePublic { get; set; }
        [DataMember]
        public string Reason { get; set; }
    }
}