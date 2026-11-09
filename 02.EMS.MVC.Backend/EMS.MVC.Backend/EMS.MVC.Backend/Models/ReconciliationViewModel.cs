using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Backend.Models
{
    [Serializable]
    [DataContract]
    public class ReconciliationViewModel
    {
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string FromDate { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string CustomerCode { get; set; }
        [DataMember]
        public string CustomerPPCRM { get; set; }
        [DataMember]
        public string PostOffice { get; set; }
        [DataMember]
        public string ReportType { get; set; }
        [DataMember]
        public List<SelectListItem> ListReportType { get; set; }
    }
}