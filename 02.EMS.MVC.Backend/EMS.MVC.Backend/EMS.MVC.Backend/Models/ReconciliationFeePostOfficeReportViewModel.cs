using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.MVC.Backend.Models
{
    public class ReconciliationFeePostOfficeReportViewModel
    {
        public string CustomerPPCRM { get; set; }
        public long TotalOrder { get; set; }
        public long TotalWeight { get; set; }
        public long TotalFee { get; set; }
        public string TypeItemName { get; set; }
    }
}