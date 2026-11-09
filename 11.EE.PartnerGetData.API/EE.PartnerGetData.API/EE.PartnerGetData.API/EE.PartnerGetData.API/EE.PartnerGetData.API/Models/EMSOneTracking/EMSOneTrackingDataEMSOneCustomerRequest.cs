using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EE.PartnerGetData.API.Models.EMSOneTracking
{
    public class EMSOneTrackingDataEMSOneCustomerRequest
    {
        public string Data { get; set; }
    }

    public class EMSOneTrackingDataEMSOneCustomerResponse
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Code { get; set; }
        public string CustomerCode { get; set; }
        public string Status { get; set; }
        public string IsLock { get; set; }
    }
}