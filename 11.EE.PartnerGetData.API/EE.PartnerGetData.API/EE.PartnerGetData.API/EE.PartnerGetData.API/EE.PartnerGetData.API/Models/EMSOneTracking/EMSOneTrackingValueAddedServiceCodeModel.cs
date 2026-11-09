using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingValueAddedServiceCodeModel
    {
        public string ItemCode { get; set; }
        //public string ServiceCode { get; set; }
        public string ValueAddedServiceCode { get; set; }
        public long Freight { get; set; }
        //public string PhaseCode { get; set; }
        public string AddedDate { get; set; }
        public long FreightVAT { get; set; }
        public string PosCode { get; set; }
        public long OriginalFreight { get; set; }
        public long OriginalFreightVAT { get; set; }
        public string CreateTime { get; set; }
        public string LastUpdatedTime { get; set; }
    }
}