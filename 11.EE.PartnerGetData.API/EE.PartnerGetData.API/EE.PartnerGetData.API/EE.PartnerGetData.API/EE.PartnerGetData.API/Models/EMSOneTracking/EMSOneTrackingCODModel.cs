using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingCODModel
    {
        public string ItemCode { get; set; }
        public string CustomerCode { get; set; }
        public string PropertyCode { get; set; }
        public long Value { get; set; }
        public string ValueAddedServiceCode { get; set; }
        public string CreateTime { get; set; }
        public string LastUpdatedTime { get; set; }
    }
}