using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace EMS.MVC.Frontend.Models
{
    [Serializable]
    [DataContract]
    public class DebtNoticesViewModel
    {
        [DataMember]
        public string ReportType { get; set; }
        [DataMember]
        public List<SelectListItem> ListReportType { get; set; }
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string CustomerPPCRM { get; set; }
    }
}