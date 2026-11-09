using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingItemStatusModel
    {
        public string ItemCode { get; set; }
        public string TraceDateTime { get; set; }
        public string TraceDate { get; set; }
        public string TimeDetail { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string PosCode { get; set; }
        public int TraceIndex { get; set; }
        public string ReasonDeliveryFailure { get; set; }
        public string CustomerCode { get; set; }
    }
}