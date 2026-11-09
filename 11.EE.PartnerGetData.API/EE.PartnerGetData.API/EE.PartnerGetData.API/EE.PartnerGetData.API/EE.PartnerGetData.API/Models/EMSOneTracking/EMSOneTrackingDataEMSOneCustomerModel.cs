using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingDataEMSOneCustomerModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }
        public string MobileNumber { get; set; }
        public string CustomerCode { get; set; }
        public string CrmOrPaypostCode { get; set; }
        public string IsLock { get; set; }
        public string DateType { get; set; }
    }
}