using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingItemStatusPickupModel
    {
        public string ItemCode { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string OrderCode { get; set; }
        //public int Quantity { get; set; }
        //public int Weight { get; set; }
        public string CustomerCode { get; set; }
        public string Note { get; set; }
        public string DateTime { get; set; }
    }
}